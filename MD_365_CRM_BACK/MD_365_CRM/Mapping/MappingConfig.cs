using AutoMapper;
using MD_365_CRM.Models;
using MD_365_CRM.Requests;

namespace MD_365_CRM.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<OpportunityRequest, Opportunity>().ReverseMap();
            CreateMap<IncidentRequest, Incident>().ReverseMap();
            CreateMap<ProductRequest, Product>().ReverseMap();
        }
    }
}
