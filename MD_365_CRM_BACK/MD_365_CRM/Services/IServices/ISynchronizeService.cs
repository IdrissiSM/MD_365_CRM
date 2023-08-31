namespace MD_365_CRM.Services.IServices
{
    public interface ISynchronizeService
    {
        //Task<bool> SynchronizeAsync(Guid contactId);

        Task<bool> SynchronizeOpportunitiesAsync(Guid contactId);

        Task<bool> SynchronizeIncidentsAsync(Guid contactId);

        Task<bool> SynchronizeProductsAsync(Guid contactId);

        Task<bool> SynchronizeProfileAsync(Guid contactId);
    }
}
