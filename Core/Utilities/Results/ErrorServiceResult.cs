using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class ErrorServiceResult<T> : ServiceResult<T>
    {
        public ErrorServiceResult()
        {
            this.Result = false;
        }
        public ErrorServiceResult(bool result, string message)
        {
            this.Result = result;
            this.Message = message;
            this.Obj = (object)null;
            this.Data = default(T);
        }
    }
}
