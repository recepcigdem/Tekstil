﻿using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        
        public static ServiceResult Run(params ServiceResult[] logics)
        {
            foreach (var logic in logics)
            {
                if (!logic.Result)
                {
                    return logic;
                }

              
            }
            return new ServiceResult(true, "Ok");
        }
    }
}
