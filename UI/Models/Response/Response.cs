namespace UI.Models.Response
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public object ResultData { get; set; }

        public Response()
        {
            IsSuccess = false;
            ErrorCode = "000";
            ErrorMessage = string.Empty;
            ResultData = null;
        }

        public Response(bool success, string code, string message, object o)
        {
            IsSuccess = success;
            ErrorCode = code;
            ErrorMessage = message;
            ResultData = o;
        }

        public Response(bool success)
        {
            IsSuccess = success;
            ErrorCode = string.Empty;
            ErrorMessage = string.Empty;
            ResultData = null;
        }
    }
}
