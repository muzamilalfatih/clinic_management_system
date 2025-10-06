namespace SharedClasses
{
   
     public class Result<T>
     {
         public bool Success { get; set; }
         public ResponseMessage  Message { get; set; }
         public int ErrorCode { get; set; }
         public T? Data { get; set; }
         public Result(bool success, string message, T? data = default, int errorCode = 0)
         {
             Success = success;
             Message = new ResponseMessage(message);
             ErrorCode = errorCode;
             Data = data;
         }
        public Result(bool success, ResponseMessage message, T? data = default, int errorCode = 0)
        {
            Success = success;
            Message = message;
            ErrorCode = errorCode;
            Data = data;
        }

    }

}
