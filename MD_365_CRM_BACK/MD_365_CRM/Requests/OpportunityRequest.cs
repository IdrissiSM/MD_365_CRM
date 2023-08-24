using MD_365_CRM.Models;

namespace MD_365_CRM.Requests
{
    public class OpportunityRequest
    {
        /// <summary>
        /// Type a subject or descriptive name, such as the expected order or company name, for the opportunity.
        /// </summary>
        public string? name { get; set; }
        /// <summary>
        /// The primary email address for the entity.
        /// </summary>
        public string? emailaddress { get; set; }
        /// <summary>
        /// Type the estimated revenue amount to indicate the potential sale or value of the opportunity for revenue forecasting. 
        /// This field can be either system-populated or editable based on the selection in the Revenue field.
        /// </summary>
        public double? estimatedvalue { get; set; }
        /// <summary>
        /// Type the actual revenue amount for the opportunity for reporting and analysis of estimated versus actual sales. 
        /// Field defaults to the Est. Revenue value when an opportunity is won.
        /// </summary>
        public double? actualvalue { get; set; }
        /// <summary>
        /// Type a number from 0 to 100 that represents the likelihood of closing the opportunity. 
        /// This can aid the sales team in their efforts to convert the opportunity in a sale.
        /// </summary>
        public int? closeprobability { get; set; }
        /// <summary>
        /// Select the expected value or priority of the opportunity based on revenue, customer status, or closing probability.
        /// 1	Hot
        /// 2	Warm
        /// 
        /// </summary>
        public int? opportunityratingcode { get; set; }
        /// <summary>
        /// Select whether an internal review has been completed for this opportunity.
        /// </summary>
        public bool? completeinternalreview { get; set; }
        /// <summary>
        /// Shows the current phase in the sales pipeline for the opportunity. This is updated by a workflow.
        /// </summary>
        public string? stepname { get; set; }
        /// <summary>
        /// Type notes about the proposed solution for the opportunity.
        /// </summary>
        public string? proposedsolution { get; set; } = String.Empty;
        /// <summary>
        /// Type some notes about the customer's requirements, 
        /// to help the sales team identify products and services that could meet their requirements.
        /// </summary>
        public string? customerneed { get; set; } = String.Empty;
        /// <summary>
        /// Type notes about the company or organization associated with the opportunity.
        /// </summary>
        public string? currentsituation { get; set; } = String.Empty;
        /// <summary>
        /// Type additional information to describe the opportunity, 
        /// such as possible products to sell or past purchases from the customer.
        /// </summary>
        public string? description { get; set; } = String.Empty;
        /// <summary>
        /// Shows who created the record on behalf of another user.
        /// </summary>
    }
}
