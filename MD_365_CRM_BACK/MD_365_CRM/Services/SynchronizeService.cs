using AutoMapper;
using MD_365_CRM.Context;
using MD_365_CRM.CRM;
using MD_365_CRM.Models;
using MD_365_CRM.Requests;
using MD_365_CRM.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MD_365_CRM.Services
{
    public class SynchronizeService : ISynchronizeService
    {
        private readonly DynamicsCRM dynamicsCRM;
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;
        private readonly ApplicationDbContext _dbContext;
        private readonly IOpportunityService _opportunityService;
        private readonly IIncidentService _incidentService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public SynchronizeService(IIncidentService incidentService, IProductService productService, IMapper mapper, DynamicsCRM crmConfig, IConfiguration configuration, ApplicationDbContext DbContext, IOpportunityService opportunityService)
        {
            _productService = productService;
            _incidentService = incidentService;
            _mapper = mapper;
            _dbContext = DbContext;
            _configuration = configuration;
            dynamicsCRM = crmConfig;
            _opportunityService = opportunityService;
            baseUrl = $"{_configuration.GetValue<string>("DynamicsCrmSettings:Scope")}/api/data/v9.2";
        }

        //async public Task<bool> SynchronizeAsync(Guid contactId)
        //{
        //    // Opportunity
        //    var UnsynchronizedOpportunities = _dbContext.Opportunities.Where(opportunity => !opportunity.IsSynchronized).ToList();
        //    var isSuccess = true;
        //    foreach(Opportunity opportunity in UnsynchronizedOpportunities){
        //        OpportunityRequest opportunityRequest = _mapper.Map<OpportunityRequest>(opportunity);
        //        isSuccess = await _opportunityService.UpdateOpportunity(opportunity.OpportunityId, opportunityRequest);
        //        opportunity.IsSynchronized = isSuccess;
        //        if (!isSuccess) return isSuccess;
        //    }

        //    // Incident
        //    var UnsynchronizedIncidents = _dbContext.Incidents.Where(inc => !inc.IsSynchronized).ToList();
        //    foreach (Incident incident in UnsynchronizedIncidents)
        //    {
        //        IncidentRequest incidentRequest = _mapper.Map<IncidentRequest>(incident);
        //        isSuccess = await _incidentService.UpdateIncident(incident.incidentid, incidentRequest);
        //        incident.IsSynchronized = isSuccess;
        //        if (!isSuccess) return isSuccess;
        //    }

        //    // Product
        //    //var UnsynchronizedProducts = _dbContext.Products.Where(p => !p.IsSynchronized).ToList();
        //    //foreach (Product product in UnsynchronizedProducts)
        //    //{
        //    //    ProductRequest productRequest = _mapper.Map<ProductRequest>(product);
        //    //    isSuccess = await _productService.UpdateProduct(product.ProductId, productRequest);
        //    //    if (!isSuccess) return isSuccess;
        //    //}

        //    User user = _dbContext.Users.SingleOrDefault(user => user.ContactId == contactId);

        //    HttpResponseMessage response = null;

        //    if (user is not null && !user.IsSynchronized)
        //    {

        //        var body = new
        //        {
        //            statuscode = user.Statecode,
        //            gendercode = user.Gendercode,
        //            jobtitle = user.Jobtitle,
        //            emailaddress1 = user.Email,
        //            lastname = user.Lastname,
        //            firstname = user.Firstname,
        //            fullname = user.UserName,
        //            contactid = user.ContactId,
        //        };

        //        string accessToken = await dynamicsCRM.GetAccessTokenAsync();

        //        response = await dynamicsCRM.CrmRequest(
        //            httpMethod: HttpMethod.Patch,
        //            accessToken: accessToken,
        //            requestUri: $"{_configuration.GetValue<string>("DynamicsCrmSettings:Scope")}/api/data/v9.2/contacts({contactId})",
        //            body: JsonConvert.SerializeObject(body));

        //        Console.WriteLine("response: "+ accessToken);
        //    }

        //    if (response != null && response.IsSuccessStatusCode) user!.IsSynchronized = true;

        //    user.IsSynchronized = true;

        //    _dbContext.SaveChanges();

        //    return isSuccess && user.IsSynchronized || (response != null && response.IsSuccessStatusCode);

        //}

        async public Task<bool> SynchronizeOpportunitiesAsync(Guid contactId)
        {
            User user = _dbContext.Users.SingleOrDefault(user => user.ContactId == contactId);

            if (user is null) return false;

            var UnsynchronizedOpportunities = _dbContext.Opportunities.Where(opportunity => !opportunity.IsSynchronized).ToList();
            var isSuccess = true;
            foreach (Opportunity opportunity in UnsynchronizedOpportunities)
            {
                OpportunityRequest opportunityRequest = _mapper.Map<OpportunityRequest>(opportunity);
                isSuccess = await _opportunityService.UpdateOpportunity(opportunity.OpportunityId, opportunityRequest);
                opportunity.IsSynchronized = isSuccess;
                if (!isSuccess) return isSuccess;
            }

            return isSuccess;
        }

        async public Task<bool> SynchronizeIncidentsAsync(Guid contactId)
        {
            User user = _dbContext.Users.SingleOrDefault(user => user.ContactId == contactId);

            if (user is null) return false;

            var UnsynchronizedIncidents = _dbContext.Incidents.Where(inc => !inc.IsSynchronized).ToList();
            var isSuccess = true;
            foreach (Incident incident in UnsynchronizedIncidents)
            {
                IncidentRequest incidentRequest = _mapper.Map<IncidentRequest>(incident);
                isSuccess = await _incidentService.UpdateIncident(incident.incidentid, incidentRequest);
                incident.IsSynchronized = isSuccess;
                if (!isSuccess) return isSuccess;
            }

            return isSuccess;
        }

        async public Task<bool> SynchronizeProductsAsync(Guid contactId)
        {
            User user = _dbContext.Users.SingleOrDefault(user => user.ContactId == contactId);

            if (user is null) return false;

            var UnsynchronizedProducts = _dbContext.Products.Where(p => !p.IsSynchronized).ToList();
            var isSuccess = true;
            foreach (Product product in UnsynchronizedProducts)
            {
                ProductRequest productRequest = _mapper.Map<ProductRequest>(product);
                isSuccess = await _productService.UpdateProduct(product.ProductId, productRequest);
                product.IsSynchronized = isSuccess;
                if (!isSuccess) return isSuccess;
            }

            return isSuccess;
        }

        async public Task<bool> SynchronizeProfileAsync(Guid contactId)
        {
            User user = _dbContext.Users.SingleOrDefault(user => user.ContactId == contactId);

            if (user is null) return false;

            HttpResponseMessage response = null;

            if (!user.IsSynchronized)
            {

                var body = new
                {
                    statuscode = user.Statecode,
                    gendercode = user.Gendercode,
                    jobtitle = user.Jobtitle,
                    emailaddress1 = user.Email,
                    lastname = user.Lastname,
                    firstname = user.Firstname,
                    fullname = user.UserName,
                    contactid = user.ContactId,
                };

                string accessToken = await dynamicsCRM.GetAccessTokenAsync();

                response = await dynamicsCRM.CrmRequest(
                    httpMethod: HttpMethod.Patch,
                    accessToken: accessToken,
                    requestUri: $"{_configuration.GetValue<string>("DynamicsCrmSettings:Scope")}/api/data/v9.2/contacts({contactId})",
                    body: JsonConvert.SerializeObject(body));

                Console.WriteLine("response: " + accessToken);
            }

            if (response != null && response.IsSuccessStatusCode)
            {
                user!.IsSynchronized = true;

                _dbContext.SaveChanges();
            }

            return user.IsSynchronized || response != null && response.IsSuccessStatusCode;
        }
    }
}
