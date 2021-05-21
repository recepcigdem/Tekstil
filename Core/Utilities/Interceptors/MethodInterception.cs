using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Core.Extensions;
using Core.Utilities.Results;
using FluentValidation;
using FluentValidation.Results;

namespace Core.Utilities.Interceptors
{
    public abstract class MethodInterception : MethodInterceptionBaseAttribute
    {

        protected virtual void OnBefore(IInvocation invocation) { }
        protected virtual void OnAfter(IInvocation invocation) { }
        protected virtual void OnException(IInvocation invocation, System.Exception e)
        {
           // Invocation = invocation;
            Result = false;
           // Message = e.Message;
            //throw e;
            //invocation.Proceed();

            Message = "Error_SystemError";
            IEnumerable<ValidationFailure> errors;
            if (e.GetType() == typeof(ValidationException))
            {
                String str = e.Message;
                var length = str.Length;
                var lastLength = str.LastIndexOf(" ");
                var sonuc = str.Substring(lastLength, length - lastLength).Trim();

                Message = sonuc;
                errors = ((ValidationException)e).Errors;

                new ValidationErrorDetails()
                {
                    StatusCode = 400,
                    Message = Message,
                    Errors = errors
                };

                invocation.Proceed();
            }

            new ErrorDetails
            {
                StatusCode = 100,
                Message = Message
            };

            invocation.Proceed();
        }

        protected virtual void OnSuccess(IInvocation invocation) { }
        public override void Intercept(IInvocation invocation)
        {
            if (Result == false)
            {
                invocation.Proceed();
            }
            OnBefore(invocation);
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                OnException(invocation, e);
            }
            finally
            {
                if (Result)
                {
                    OnSuccess(invocation);
                }
            }
            OnAfter(invocation);
        }
    }
}