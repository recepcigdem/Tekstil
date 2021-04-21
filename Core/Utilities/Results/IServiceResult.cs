using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public interface IServiceResult<T>
    {
        public bool Result { get; set; }

        public string Message { get; set; }

        public object Obj { get; set; }

        public T Data { get; set; }
    }
}
