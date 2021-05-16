using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Results;

namespace Core.Aspects.Autofac.Transaction
{
    public class TransactionScopeAspect : MethodInterception
    {
        public static int Number { get; set; }
        public static string Message { get; set; }

        public override void Intercept(IInvocation invocation)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    Message = "";
                    invocation.Proceed();
                    transactionScope.Complete();
                }
                catch (System.Exception e)
                {
                    
                    Message = e.Message;
                    transactionScope.Dispose();
                   
                }
               
            }
        }
    }
}
