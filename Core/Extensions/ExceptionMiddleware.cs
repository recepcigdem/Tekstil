using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Results;

namespace Core.Extensions
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;
       
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
          
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
               
                HandleExceptionAsync(httpContext, e);
            }
        }

        private void HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            
            IInvocation invocation=MethodInterceptionBaseAttribute.Invocation;
            
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message = "Error_SystemError";
            IEnumerable<ValidationFailure> errors;
            if (e.GetType() == typeof(ValidationException))
            {
                String str = e.Message;
                var length = str.Length;
                var lastLength = str.LastIndexOf(" ");
                var sonuc = str.Substring(lastLength, length - lastLength).Trim();


                message = sonuc;
                errors = ((ValidationException)e).Errors;
                httpContext.Response.StatusCode = 400;

               var taskValidation= httpContext.Response.WriteAsync(new ValidationErrorDetails()
                {
                    StatusCode = 400,
                    Message = message,
                    Errors = errors
                }.ToString());

               invocation.Proceed();

               //return taskValidation;
            }

            var task= httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = message
            }.ToString());

            invocation.Proceed();

            //return task;
        }
    }
}
