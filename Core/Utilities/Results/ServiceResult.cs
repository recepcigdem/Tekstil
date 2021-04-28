using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class ServiceResult :IServiceResult
    {

        public ServiceResult()
        {
            this.Result = false;
            this.Message = string.Empty;
            this.Obj = (object)null;
        }

        public ServiceResult(bool result, string message)
        {
            this.Result = result;
            this.Message = message;
            this.Obj = (object)null;
           
        }
        public ServiceResult(bool result, string message,object obj)
        {
            this.Result = result;
            this.Message = message;
            this.Obj = obj;

        }

        public bool Result { get; set; }
        public string Message { get; set; }
        public object Obj { get; set; }
        
    }
}
