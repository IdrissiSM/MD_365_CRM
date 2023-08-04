namespace MD_365_CRM.Requests
{
    public class OpportunityRequest
    {
        public string? OpportunityId { get; set; }
        public string? Name { get; set; }
        public string? EmailAddress { get; set; }
        public DateTime EstimatedCloseDate { get; set; }
        public double TotalLineItemAmount { get; set; }
        public int CloseProbability { get; set; }
        public int StatusCode { get; set; }
        public string ProposedSolution { get; set; } = String.Empty;
        public string CustomerNeed { get; set; } = String.Empty;
        public string CurrentSituation { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid _ParentAccountid_Value { get; set; }
    }
}
