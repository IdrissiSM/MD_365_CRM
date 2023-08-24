namespace MD_365_CRM.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string? ProductNumber { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ProductStructure { get; set; }
        public string? Price { get; set; }
        public string? Price_Base { get; set; }
        public string? ProductTypeCode { get; set; }
        public string? QuantityOnHand { get; set; }
        public string? StandardCost { get; set; }
        public string? CurrentCost { get; set; }
        public string? CurrentCost_Base { get; set; }
        public string? StandardCost_Base { get; set; }
        public int? StateCode { get; set; }
        public int? StatusCode { get; set; }
        public string? StockVolume { get; set; }
        public string? ExchangeRate { get; set; }
        public string? VersionNumber { get; set; }
        public DateTime? ValidFromDate { get; set; }
        public bool IsSynchronized { get; set; } = true;
        public DateTime? ValidToDate { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
