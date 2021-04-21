using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {

        public bool Success { get; }
        public string Message { get; set; }
        public object Obj { get; set; }

        public Result()
        {
            Success = false;
            Message = string.Empty;
            Obj = null;
        }

        public Result(bool success)
        {
            Success = success;
        }
        public Result(object obj, bool success) : this(success)
        {
            Obj = obj;
        }

        public Result(bool success, string message) : this(success)
        {
            Message = message;
        }
        public Result(object obj, bool success, string message) : this(obj,success)
        {
            Message = message;
        }

        
    }
}
