using Azure.Core;
using MD_365_CRM.CRM;
using MD_365_CRM.Models;
using MD_365_CRM.Requests;
using MD_365_CRM.Responses;
using MD_365_CRM.Services.IServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MD_365_CRM.Services
{
    public class ProductService : IProductService
    {
        private readonly DynamicsCRM _dynamicsCRM;
        private string _baseUrl;
        private readonly IOpportunityService _opportunityService;

        public ProductService(DynamicsCRM dynamicsCRM, IConfiguration configuration, IOpportunityService opportunityService)
        {
            _dynamicsCRM = dynamicsCRM;
            _baseUrl = $"{configuration.GetValue<string>("DynamicsCrmSettings:Scope")}api/data/v9.2";
            _opportunityService = opportunityService;
        }

        public async Task<List<Product>> GetProducts()
        {
            try
            {
                var accessToken = await _dynamicsCRM.GetAccessTokenAsync();

                var response1 = await _dynamicsCRM.CrmRequest(
                    HttpMethod.Get,
                    accessToken,
                    $"{_baseUrl}/opportunities?$filter=_customerid_value eq 81883308-7ad5-ea11-a813-000d3a33f3b4&$expand=product_opportunities");

                var json1 = await response1.Content.ReadAsStringAsync();
                var result1 = JsonConvert.DeserializeObject<dynamic>(json1);
                List<Opportunity> opportunities = result1?.value.ToObject<List<Opportunity>>();
                List<Guid> productIds = opportunities.SelectMany(opportunity =>
                    opportunity.Product_opportunities.Select(productOpportunity => productOpportunity._productId_value)
                ).ToList();
                var filter = string.Join(" or ", productIds.Select(id => $"productid eq {id}"));
                var response2 = await _dynamicsCRM.CrmRequest(
                    HttpMethod.Get,
                    accessToken,
                    $"{_baseUrl}/products?$filter={filter}");

                var json2 = await response2.Content.ReadAsStringAsync();
                var result2 = JsonConvert.DeserializeObject<dynamic>(json2);
                List<Product> products = result2?.value.ToObject<List<Product>>();

                return products!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetProducts: {ex.Message}");
                return null;
            }
        }
        public async Task<Product> GetProductById(Guid id)
        {
            try
            {
                var accessToken = await _dynamicsCRM.GetAccessTokenAsync();
                var response = await _dynamicsCRM.CrmRequest(
                    HttpMethod.Get,
                    accessToken,
                    $"{_baseUrl}/products({id})");

                var json = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Product>(json);

                return product!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetProductById: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<BestSellingProducts>> GetBestSellingProducts()
        {
            {
                List<Opportunity> opportunities = await this._opportunityService.GetOpportunities();
                List<ProductOpportunity> productOpportunities = new List<ProductOpportunity>();

                opportunities.ForEach(o =>
                {
                    o.Product_opportunities.ForEach(x =>
                    {
                        productOpportunities.Add(x);
                    });
                });

                IEnumerable<BestSellingProducts> bestSellingProducts = productOpportunities.GroupBy(o => o.ProductName)
                    .Select(group => new BestSellingProducts()
                    {
                        ProductName = group.Key,
                        Quantity = group.Select(x => x.Quantity).Sum(),
                        PricePerUnit = group.First(x => true).PricePerUnit
                    })
                    .OrderByDescending(p => p.Quantity).Take(6);
                return bestSellingProducts;
            }
        }
    }
}
