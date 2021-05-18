using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Core.CrossCuttingConcerns.Validation
{
    public static class ValidationTool
    {
        //public static IServiceResult Validate(IValidator validator, object entity)
        //{
        //    var context = new ValidationContext<object>(entity);

        //    var result = validator.Validate(context);

        //    if (!result.IsValid)
        //    {
        //        return new ServiceResult(false, result.ToString());
        //    }
        //    return null;
        //}

        public static void Validate(IValidator validator, object entity)
        {
            var context = new ValidationContext<object>(entity);

            var result = validator.Validate(context);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}
