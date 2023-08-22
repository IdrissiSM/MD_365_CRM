namespace MD_365_CRM.Requests
{
    public class UpdateProfileRequest
    {
        public string Username { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public int GenderCode { get; set; }

        public string JobTitle { get; set; }

        public string Email { get; set; }

        public Guid ContactId { get; set; }
    }
}
