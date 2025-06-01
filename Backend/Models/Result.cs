namespace Backend.Models
{
    public class Result<T>
    {
        public bool Succeeded { get; private set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static Result<T> Success(T data, string message = "")
        {
            return new Result<T>
            {
                Succeeded = true,
                Message = message,
                Data = data
            };
        }

        public static Result<T> Failure(string message)
        {
            return new Result<T>
            {
                Succeeded = false,
                Message = message,
            };
        }
    }
}