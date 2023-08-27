using System.Numerics;

namespace MD_365_CRM.Requests
{
    public class ProductRequest
    {
        public string? productnumber { get; set; } = string.Empty;
        public string? name { get; set; }
        public string? description { get; set; }
        public string? productstructure { get; set; }
        public double? price { get; set; }
        public double? price_base { get; set; }
        public int? producttypecode { get; set; }
        public double? quantityonhand { get; set; }
        public double? standardcost { get; set; }
        public double? currentcost { get; set; }
        public double? currentcost_base { get; set; }
        public double? standardcost_base { get; set; }
        public double? stockvolume { get; set; }
        public double? exchangerate { get; set; }
    }
}
