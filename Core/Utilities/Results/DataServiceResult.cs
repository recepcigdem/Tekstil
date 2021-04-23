using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class DataServiceResult<T> :IDataServiceResult<T>
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public object Obj { get; set; }
        public T Data { get; set; }

        public DataServiceResult()
        {
            this.Result = true;
        }
       public DataServiceResult(bool result, string message)
        {
            this.Result = result;
            this.Message = message;
            this.Obj = (object)null;
        }
       public DataServiceResult(T data, bool result, string message)
       {
           this.Result = result;
           this.Message = message;
           this.Obj = (object)null;
           this.Data = data;
       }
       public DataServiceResult(T data, object obj,bool result, string message)
       {
           this.Result = result;
           this.Message = message;
           this.Obj =obj;
           this.Data = data;
       }

    }
}
