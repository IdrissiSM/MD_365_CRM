namespace MD_365_CRM.Models
{
    public class Contact
    {
        public Guid contactId { get; set; }
        public string fullname { get; set; } = String.Empty;
        public string firstname { get; set; } = String.Empty;
        public string lastname { get; set; } = String.Empty;
        public string emailaddress1 { get; set; } = String.Empty;
        public string jobtitle { get; set; } = String.Empty;
        public Int32? gendercode { get; set; } = Int32.MinValue;
        public int? statuscode { get; set; } = 0;
        public string secret { get; set;} = String.Empty;
        public int[]? Image { get; set; } = null;
    }
}
