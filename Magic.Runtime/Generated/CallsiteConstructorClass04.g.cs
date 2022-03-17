// this file was generated by Magic.Runtime.Callsites -- do not edit it by hand!
using System;
using System.Reflection;

namespace Magic
{
    public class CallsiteConstructor04<T>
    {
        CallSiteCache04 cache;

        public CallsiteConstructor04()
        {
            cache = new CallSiteCache04();
        }

        public T Invoke(object arg0,object arg1,object arg2,object arg3)
        {
            if(cache.TryGet(arg0,arg1,arg2,arg3, out var result))
                return (T)result(arg0,arg1,arg2,arg3);

            var ctor = Dispatch.BindToConstructor(typeof(T), new [] { arg0,arg1,arg2,arg3 });
            if (ctor != null)
            {
                cache.CacheMethod(arg0,arg1,arg2,arg3, DelegateHelpers.GetMethodDelegate04(ctor));
                var args = new[] { arg0,arg1,arg2,arg3 };
                Binder.Shared.ConvertArguments(ctor, args);
                return (T)Dispatch.InvokeUnwrappingExceptions(ctor, null, args);
            }
            throw new ArgumentException($"Could not invoke constructor `{typeof(T)}` with types {arg0.GetType()}, {arg1.GetType()}, {arg2.GetType()}, {arg3.GetType()}.");
        }
    }
}