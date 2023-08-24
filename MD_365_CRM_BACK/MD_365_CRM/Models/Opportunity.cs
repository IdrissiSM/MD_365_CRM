namespace MD_365_CRM.Models
{
    public class Opportunity
    {
        /// <summary>
        /// Unique identifier of the opportunity.
        /// </summary>
        public Guid OpportunityId { get; set; }
        /// <summary>
        /// Type a subject or descriptive name, such as the expected order or company name, for the opportunity.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// The primary email address for the entity.
        /// </summary>
        public string? EmailAddress { get; set; }
        /// <summary>
        /// Type the estimated revenue amount to indicate the potential sale or value of the opportunity for revenue forecasting. 
        /// This field can be either system-populated or editable based on the selection in the Revenue field.
        /// </summary>
        public double? EstimatedValue { get; set; }
        /// <summary>
        /// Type the actual revenue amount for the opportunity for reporting and analysis of estimated versus actual sales. 
        /// Field defaults to the Est. Revenue value when an opportunity is won.
        /// </summary>
        public double? ActualValue { get; set; }
        /// <summary>
        /// The expected closing date of the opportunity to help make accurate revenue forecasts.
        /// </summary>
        public DateTime? EstimatedCloseDate { get; set; }
        /// <summary>
        /// Shows the date and time when the opportunity was closed or canceled.
        /// </summary>
        public DateTime? ActualCloseDate { get; set; }
        /// <summary>
        /// Type a number from 0 to 100 that represents the likelihood of closing the opportunity. 
        /// This can aid the sales team in their efforts to convert the opportunity in a sale.
        /// </summary>
        public int? CloseProbability { get; set; }
        /// <summary>
        /// Select the opportunity's status. 
        /// 1	In Progress
        /// 2	On Hold
        /// 3	Won
        /// 4	Canceled
        /// 5	Out-Sold	
        /// </summary>
        public int? StatusCode { get; set; }
        /// <summary>
        /// Shows whether the opportunity is open, won, or lost.
        /// Won and lost opportunities are read-only and can't be edited until they are reactivated.
        /// 0	Open
        /// 1	Won
        /// 2	Lost
        /// </summary>
        public int? StateCode { get; set; }
        /// <summary>
        /// Select the expected value or priority of the opportunity based on revenue, customer status, or closing probability.
        /// 1	Hot
        /// 2	Warm
        /// 
        /// </summary>
        public int? OpportunityRatingCode { get; set; }
        /// <summary>
        /// Select whether an internal review has been completed for this opportunity.
        /// </summary>
        public bool? CompleteInternalReview { get; set; }
        /// <summary>
        /// Shows the current phase in the sales pipeline for the opportunity. This is updated by a workflow.
        /// </summary>
        public string? StepName { get; set; }
        /// <summary>
        /// Type notes about the proposed solution for the opportunity.
        /// </summary>
        public string? ProposedSolution { get; set; } = String.Empty;
        /// <summary>
        /// Type some notes about the customer's requirements, 
        /// to help the sales team identify products and services that could meet their requirements.
        /// </summary>
        public string? CustomerNeed { get; set; } = String.Empty;
        /// <summary>
        /// Type notes about the company or organization associated with the opportunity.
        /// </summary>
        public string? CurrentSituation { get; set; } = String.Empty;
        /// <summary>
        /// Type additional information to describe the opportunity, 
        /// such as possible products to sell or past purchases from the customer.
        /// </summary>
        public string? Description { get; set; } = String.Empty;
        /// <summary>
        /// Shows who created the record on behalf of another user.
        /// </summary>
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// Date and time when the record was modified.
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
        /// <summary>
        /// Account ID
        /// </summary>
        public Guid? _parentAccountId_value { get; set; }
        /// <summary>
        /// Contact ID
        /// </summary>
        public Guid? _parentContactId_value { get; set; }
        /// <summary>
        /// The list of associated products that the opportunity contains
        /// </summary>
        public List<ProductOpportunity>? Product_opportunities { get; set; }
        /// <summary>
        /// Prperties to use in GET request to Microsoft Dynamics CRM
        /// </summary>
        public bool IsSynchronized { get; set; } = true;

        public static string  Properties = "name,emailaddress,estimatedclosedate,actualclosedate," +
            " estimatedvalue,actualvalue,totaltax,closeprobability,statecode,statuscode,opportunityratingcode," +
            "completeinternalreview,stepname,filedebrief,proposedsolution,customerneed," +
            "currentsituation,description,createdon,modifiedon,_parentaccountid_value,_parentcontactid_value";
    }
}
