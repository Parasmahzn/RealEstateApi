namespace RealEstateApi.Models
{
    public class CommonResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public List<Errors>? Errors { get; set; }

        public object? Data { get; set; }
    }
    public class Errors
    {
        public ErrorCodes ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public enum ErrorCodes
    {
        DbError = 100,
        Unauthorized = 400,
        DuplicateRecord = 101,
        BadRequest = 401,
        NotFound = 404,
        ServerError = 500
    }

    public enum Message
    {
        Success = 0,
        Failed = 1,
        Pending = 2
    }
}
