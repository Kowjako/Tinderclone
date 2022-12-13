namespace DatingAppAPI.Application.Errors
{
    public class DatingException : Exception 
    {
        public int StatusCode { get; protected set; }
        public string Message { get; protected set; }
    }

    public class HttpException : DatingException
    {
        public HttpException(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
