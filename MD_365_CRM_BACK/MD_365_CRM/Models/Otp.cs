namespace MD_365_CRM.Models
{
    public class Otp
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public DateTime CreationDate { get; set; }

        public int Value { get; set; }
    }
}
