namespace Snap.APIs.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public IEnumerable<string> Erorrs { get; set; }
        public ApiValidationErrorResponse(): base (400)
        {
            
        }

    }
}
