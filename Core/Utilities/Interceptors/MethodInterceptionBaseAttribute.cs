using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;
using Core.Utilities.Results;

namespace Core.Utilities.Interceptors
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class MethodInterceptionBaseAttribute : Attribute, IInterceptor,IServiceResult
    {
        public int Priority { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public object Obj { get; set; }

        public virtual void Intercept(IInvocation invocation)
        {

        }

       
    }
}
