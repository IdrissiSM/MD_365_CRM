using Azure.Core;
using MD_365_CRM.Context;
using MD_365_CRM.CRM;
using MD_365_CRM.Models;
using MD_365_CRM.Requests;
using MD_365_CRM.Responses;
using MD_365_CRM.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MD_365_CRM.Services
{
    public class ProductService : IProductService
    {
        private readonly DynamicsCRM _dynamicsCRM;
        private readonly UserManager<User> _userManager;
        private readonly IOpportunityService _opportunityService;
        private string _baseUrl;
        private readonly ApplicationDbContext _dbContext;

        public ProductService(DynamicsCRM dynamicsCRM, IConfiguration configuration, UserManager<User> userManager, IOpportunityService opportunityService, ApplicationDbContext DbContext)
        {
            _dbContext = DbContext;
            _dynamicsCRM = dynamicsCRM;
            _baseUrl = $"{configuration.GetValue<string>("DynamicsCrmSettings:Scope")}api/data/v9.2";
            _userManager = userManager;
            _opportunityService = opportunityService;
        }

        public async Task<List<Product>> GetProducts(Guid contactId)
        {
            try
            {
                var AllRecords = _dbContext.Products.ToList();
                if (AllRecords.Count() > 0)
                {
                    return AllRecords;
                }

                var accessToken = await _dynamicsCRM.GetAccessTokenAsync();
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.ContactId == contactId);

                if (user == null)
                    return null;
                bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                var response = new HttpResponseMessage();
                if (!isAdmin)
                {
                    var response1 = await _dynamicsCRM.CrmRequest(
                    HttpMethod.Get,
                    accessToken,
                    $"{_baseUrl}/opportunities?$filter=_customerid_value eq {contactId}&$expand=product_opportunities");

                    var json1 = await response1.Content.ReadAsStringAsync();
                    var result1 = JsonConvert.DeserializeObject<dynamic>(json1);
                    List<Opportunity> opportunities = result1?.value.ToObject<List<Opportunity>>();



                    List<Guid> productIds = opportunities.SelectMany(opportunity =>
                        opportunity.Product_opportunities.Select(productOpportunity => productOpportunity._productId_value)
                    ).ToList();
                    var filter = string.Join(" or ", productIds.Select(id => $"productid eq {id}"));
                    response = await _dynamicsCRM.CrmRequest(
                        HttpMethod.Get,
                        accessToken,
                        $"{_baseUrl}/products?$filter={filter}");
                }
                else
                {
                    response = await _dynamicsCRM.CrmRequest(
                        HttpMethod.Get,
                        accessToken,
                        $"{_baseUrl}/products");
                }
                

                var json = await response?.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(json);
                List<Product> products = result?.value.ToObject<List<Product>>();

                if (products is not null)
                {
                    foreach (Product product in products)
                    {
                        _dbContext.Products.Add(product);
                    }
                    _dbContext.SaveChanges();
                }

                return products!;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetProducts: {ex.Message}");
                return null;
            }
        }
        public async Task<Product> GetProductById(Guid productId)
        {
            Product product = await _dbContext.Products.FindAsync(productId);
            return product!;
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


        public async Task<bool> UpdateProduct(Guid productId, ProductRequest product)
        {
            // get access token
            string accessToken = await _dynamicsCRM.GetAccessTokenAsync();
            // Set xrm request params
            var jsonProduct = JsonConvert.SerializeObject(product);
            var response = await _dynamicsCRM.CrmRequest(
                HttpMethod.Patch,
                accessToken,
                $"{_baseUrl}/products({productId})",
                jsonProduct);

            return response.IsSuccessStatusCode;
        }

    }
}
