namespace MD_365_CRM.Responses
{
    public class AuthResponse
    {
        public bool IsAuthenticated { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
