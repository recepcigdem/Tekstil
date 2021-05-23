using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Interceptors;
using Core.Utilities.Results;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Core.CrossCuttingConcerns.Validation
{
    public static class ValidationTool
    {
    
        public static void Validate(IValidator validator, object entity)
        {
            var context = new ValidationContext<object>(entity);

            var result = validator.Validate(context);

            if (!result.IsValid)
            {
                MethodInterceptionBaseAttribute.Result = false;
                MethodInterceptionBaseAttribute.Message = result.Errors[0].ErrorMessage;
                //throw new ValidationException(result.Errors);
            }
        }
    }
}
