(ns magic.faster
  (:require [magic.analyzer :as ana]
            [magic.analyzer.types :refer [clr-type non-void-clr-type tag]]
            [magic.core :as magic]
            [mage.core :as il])
  (:import [clojure.lang RT]
           [System.Reflection
            MethodAttributes]))

(defn static-type-fn
  "Symbolize :fn to a simple static type without interfaces,
  HasArity methods, or constructors"
  [{:keys [local methods] :as ast} compilers]
  (il/type
    (magic/gen-fn-name (:form local))
    (map #(magic/compile % compilers) methods)))

(defn typed-static-invoke-fn-method
  "Symbolize :fn-method to a single, well typed static method"
  [{:keys [body params] {:keys [ret]} :body} compilers]
  (let [param-types (mapv clr-type params)
        return-type (non-void-clr-type ret)]
    (il/method
      "invoke"
      (enum-or MethodAttributes/Public MethodAttributes/Static)
      return-type param-types
      [(magic/compile body compilers)
       (magic/convert (clr-type ret) return-type)
       (il/ret)])))

(defn static-argument-local
  "Symbolize :local arguments to static arguments"
  [{:keys [name arg-id local] :as ast} compilers]
  (if (= local :arg)
    (magic/load-argument ast)
    (magic/throw! "Local " name " not an argument and could not be compiled")))

;; TODO ::il/type is kind of lame, use ::il/name maybe?
(defn faster-type [args-names args-types body]
  (let [faster-compilers
        (merge magic/base-compilers
               {:fn static-type-fn
                :fn-method typed-static-invoke-fn-method
                :local static-argument-local})
        name (symbol (str "--" (gensym "faster--body") "--"))
        wrapped-body `(clojure.core/fn
                        ~name
                        ~(->> args-names
                              (map #(vary-meta %2 assoc :tag %1)
                                   args-types)
                              vec)
                        ~body)
        body-il (-> wrapped-body
                    ana/analyze
                    (magic/compile (magic/get-compilers faster-compilers)))]
    (il/emit! body-il)
    (::il/type body-il)))