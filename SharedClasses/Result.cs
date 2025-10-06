namespace SharedClasses
{
   
     public class Result<T>
     {
         public bool success { get; set; }
         public string  message { get; set; }
         public int errorCode { get; set; }
         public T? data { get; set; }
         public Result(bool success, string message, T? data = default, int errorCode = 0)
         {
             this.success = success;
             this.message = message;
             this.errorCode = errorCode;
             this.data = data;
         }
        
     }

}
