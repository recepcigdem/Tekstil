using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class SuccessDataServiceResult<T> : DataServiceResult<T>
    {
        public SuccessDataServiceResult()
        {
            this.Result = false;
        }
        public SuccessDataServiceResult(bool result, string message)
        {
            this.Result = result;
            this.Message = message;
            this.Obj = (object)null;
        }
        public SuccessDataServiceResult(T data, string message)
        {
            this.Data = data;
            this.Message = message;
            this.Obj = (object)null;
           
        }
        public SuccessDataServiceResult(T data, bool result, string message)
        {
            this.Result = result;
            this.Message = message;
            this.Obj = (object)null;
            this.Data = data;
        }
        public SuccessDataServiceResult(T data, object obj, bool result, string message)
        {
            this.Result = result;
            this.Message = message;
            this.Obj = obj;
            this.Data = data;
        }
    }
}
