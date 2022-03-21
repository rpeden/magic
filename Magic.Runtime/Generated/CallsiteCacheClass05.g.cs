// this file was generated by Magic.Runtime.Callsites -- do not edit it by hand!
using System;

namespace Magic
{
    public class CallSiteCache05
    {
        struct Signature
        {
            Type type0;
            Type type1;
            Type type2;
            Type type3;
            Type type4;
            
            public Signature(object arg0,object arg1,object arg2,object arg3,object arg4)
            {
                type0 = arg0.GetType();
                type1 = arg1.GetType();
                type2 = arg2.GetType();
                type3 = arg3.GetType();
                type4 = arg4.GetType();
            }

            public bool Match(object arg0,object arg1,object arg2,object arg3,object arg4)
            {
                return type0 == arg0.GetType() && type1 == arg1.GetType() && type2 == arg2.GetType() && type3 == arg3.GetType() && type4 == arg4.GetType();
            }
        }

        struct Entry
        {
            public Signature Signature;
            public CallsiteFunc<object, object, object, object, object, object> Function;
        }
        int cacheSize;
        int count = 0;

        // l0l1Cache[0] is l0 cache, first entry checked
        // l0l1Cache[1..cacheSize] is l1 cache, looped through to find best match
        Entry[] l0l1Cache;

        public CallSiteCache05(int cacheSize)
        {
            this.cacheSize = cacheSize;
            l0l1Cache = new Entry[cacheSize];
        }

        public CallSiteCache05()
        {
            this.cacheSize = 9;
            l0l1Cache = new Entry[cacheSize];
        }

        public void CacheMethod(object arg0, object arg1, object arg2, object arg3, object arg4, CallsiteFunc<object, object, object, object, object, object> func)
        {
            var c = count < cacheSize ? count++ : count - 1;
            l0l1Cache[c] = new Entry { Signature = new Signature(arg0,arg1,arg2,arg3,arg4), Function = func };
            // CacheSwap(0, c);
            var temp = l0l1Cache[c];
            l0l1Cache[c] = l0l1Cache[0];
            l0l1Cache[0] = temp;
        }

        public bool TryGet(object arg0, object arg1, object arg2, object arg3, object arg4, out CallsiteFunc<object, object, object, object, object, object> result)
        {
            var sig0 = l0l1Cache[0].Signature;
            var func0 = l0l1Cache[0].Function;
            if (sig0.Match(arg0,arg1,arg2,arg3,arg4))
            {
                result = func0;
                return true;
            }
            for (var i = 0; i < count; i++)
            {
                var sig = l0l1Cache[i].Signature;
                var func = l0l1Cache[i].Function;
                if (sig.Match(arg0,arg1,arg2,arg3,arg4))
                {
                    // CacheSwap(0, i);
                    var temp = l0l1Cache[i];
                    l0l1Cache[i] = l0l1Cache[0];
                    l0l1Cache[0] = temp;
                    result = func;
                    return true;
                }
            }
            result = default;
            return false;
        }
    }
}