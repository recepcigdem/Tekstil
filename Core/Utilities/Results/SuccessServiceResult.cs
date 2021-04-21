using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class SuccessServiceResult<T> :ServiceResult<T>
    {
        public SuccessServiceResult()
        {
            this.Result = true;
        }
       public SuccessServiceResult(bool result, string message)
        {
            this.Result = result;
            this.Message = message;
            this.Obj = (object)null;
            this.Data = default(T);
       }
       public SuccessServiceResult(bool result, string message, T data)
       {
           this.Result = result;
           this.Message = message;
           this.Obj = (object)null;
           this.Data = data;
       }
    }
}
