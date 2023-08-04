using MD_365_CRM.Models;

namespace MD_365_CRM.Services.IServices
{
    public interface IIncidentService
    {
        Task<List<Incident>> GetAllIncidentsAsync();
        Task<List<Incident>> GetIncidentsAsync(Guid contactId);
        Task<Incident> GetIncidentByIdAsync(Guid incidentId);
    }
}
