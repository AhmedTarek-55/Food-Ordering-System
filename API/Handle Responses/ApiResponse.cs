namespace API.Handle_Responses
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int code)
            => code switch
            {
                400 => "Bad Request",
                401 => "You are not authorized",
                404 => "Resource is Not Found",
                500 => "Internal Server Error",
                _ => null
            };
    }
}
