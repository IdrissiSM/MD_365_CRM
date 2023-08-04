using System.Net;

namespace MD_365_CRM.Responses
{
    public class APIResponse
    {
        public HttpStatusCode httpStatusCode { get; set; }
        public bool Success { get; set; } = true;
        public List<string> ErrorMessages { get; set; } = null;
        public object? Result { get; set; }

    }
}
