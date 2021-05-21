using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;
using Core.Utilities.Results;

namespace Core.Utilities.Interceptors
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class MethodInterceptionBaseAttribute : Attribute, IInterceptor
    {
        public int Priority { get; set; }
        public static bool Result { get; set; }
        public static string Message { get; set; }
        public static object Obj { get; set; }
        public static IInvocation Invocation { get; set; }

        public MethodInterceptionBaseAttribute()
        {
            Result = true;
            Message = "";
        }

        public virtual void Intercept(IInvocation invocation)
        {
            
        }

       
    }
}
