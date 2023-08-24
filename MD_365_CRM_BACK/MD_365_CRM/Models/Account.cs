namespace MD_365_CRM.Models
{
    public class Account
    {
        public Guid accountid { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public string? emailaddress1 { get; set; }
        public string? entityimage_url { get; set; }
        public decimal? revenue { get; set; }
        public int? statecode { get; set; }
        public bool IsSynchronized { get; set; } = true;

        public static string Properties()
        {
            return "accountid,name,description,emailaddress1,entityimage_url,revenue,statecode";
        }
    }
}
