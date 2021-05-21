﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new System.Exception("Bu bir doğrulama sınıfı değil");
            }
            //AspectMessages.WrongValidationType --> exception message
            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);//productvalidator newlendi.
            var entityType = _validatorType.BaseType.GetGenericArguments()[0]; //product tipi alındı.
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType); //methodun argumanlarını geziliyor.
            foreach (var entity in entities)
            {
                if (Result==false)
                {
                    return;
                }
                ValidationTool.Validate(validator, entity);
            }
        }
    }
}
