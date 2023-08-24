using System.ComponentModel.DataAnnotations;

namespace MD_365_CRM.Models
{
    public class ProductOpportunity
    {
        /// <summary>
        /// Unique identifier of the ProductOpportunity.
        /// </summary>
        [Key]
        public Guid OpportunityProductId { get; set; }
        /// <summary>
        /// Opportunity Product Name.
        /// </summary>
        public string? ProductName { get; set; }
        /// <summary>
        /// Shows the price per unit of the opportunity product, 
        /// based on the price list specified on the parent opportunity.
        /// </summary>
        public double? PricePerUnit { get; set; }
        /// <summary>
        /// Type the amount or quantity of the product the customer would like to purchase.
        /// </summary>
        public double? Quantity { get; set; }
        /// <summary>
        /// Shows who created the record on behalf of another user.
        /// </summary>
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// Unique identifier of the opportunity with which the opportunity product is associated.
        /// </summary>
        public Guid _opportunityId_value { get; set; }
        /// <summary>
        /// Choose the product to include on the opportunity to link the product's pricing and other information to the opportunity.
        /// </summary>
        public bool IsSynchronized { get; set; } = true;
        public Guid _productId_value { get; set; }
        public static string Properties = "productid,productname,priceperunit,quantity,createdon,_productid_value,_opportunityid_value";
    }
}
