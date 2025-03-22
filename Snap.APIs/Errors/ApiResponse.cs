namespace Snap.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; }
        public string Message { get; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = !string.IsNullOrWhiteSpace(message)
                ? message
                : GetDefaultMessageForStatusCode(statusCode);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode) =>
            statusCode switch
            {
                400 => "Bad Request",
                401 => "You Are Un-Authorized",
                404 => "Resource Not Found",
                500 => "Internal Server Error",
                _ => "An unexpected error occurred"
            };
    }
}
