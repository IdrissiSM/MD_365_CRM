using MD_365_CRM.Models;

namespace MD_365_CRM.Services.IServices
{
    public interface IProductService
    {
        Task<List<Product>> GetProducts(Guid contactId);
        Task<Product> GetProductById(Guid id);
    }
}
