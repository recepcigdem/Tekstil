using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class ServiceResult<T> :IServiceResult<T>
    {
      
        public ServiceResult()
        {
            this.Result = false;
            this.Message = string.Empty;
            this.Obj = (object)null;
            this.Data = default(T);
        }

        public ServiceResult(bool result, string message)
        {
            this.Result = result;
            this.Message = message;
            this.Obj = (object)null;
            this.Data = default(T);
        }
        public ServiceResult(bool result, string message,T data)
        {
            this.Result = result;
            this.Message = message;
            this.Obj = (object)null;
            this.Data = data;
        }

        public bool Result { get; set; }
        public string Message { get; set; }
        public object Obj { get; set; }
        public T Data { get; set; }
    }
}
