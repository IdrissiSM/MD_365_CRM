namespace MD_365_CRM.Requests
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public int Otp { get; set; }
    }
}
