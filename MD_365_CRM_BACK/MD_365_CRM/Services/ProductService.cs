using Azure.Core;
using MD_365_CRM.CRM;
using MD_365_CRM.Models;
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
        private string _baseUrl;

        public ProductService(DynamicsCRM dynamicsCRM, IConfiguration configuration, UserManager<User> userManager)
        {
            _dynamicsCRM = dynamicsCRM;
            _baseUrl = $"{configuration.GetValue<string>("DynamicsCrmSettings:Scope")}api/data/v9.2";
            _userManager = userManager;
        }

        public async Task<List<Product>> GetProducts(Guid contactId)
        {
            try
            {
                var accessToken = await _dynamicsCRM.GetAccessTokenAsync();
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.ContactId == contactId);

                if (user == null)
                    return null;
                bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

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
                    var response2 = await _dynamicsCRM.CrmRequest(
                        HttpMethod.Get,
                        accessToken,
                        $"{_baseUrl}/products?$filter={filter}");

                    var json2 = await response2.Content.ReadAsStringAsync();
                    var result2 = JsonConvert.DeserializeObject<dynamic>(json2);
                    List<Product> products1 = result2?.value.ToObject<List<Product>>();

                    return products1!;
                }
                var response = await _dynamicsCRM.CrmRequest(
                        HttpMethod.Get,
                        accessToken,
                        $"{_baseUrl}/products");

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(json);
                List<Product> products = result?.value.ToObject<List<Product>>();

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
    }
}
