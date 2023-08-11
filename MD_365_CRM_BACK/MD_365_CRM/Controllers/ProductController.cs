using MD_365_CRM.Models;
using MD_365_CRM.Responses;
using MD_365_CRM.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MD_365_CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("GetProducts/{contactId}")]
        public async Task<IActionResult> GetProducts(Guid contactId)
        {
            APIResponse response = new();
            List<Product> products = await _productService.GetProducts(contactId); 
            //if (products is null)
            //{
            //    response.httpStatusCode = HttpStatusCode.NotFound;
            //    response.Success = false;
            //    response.ErrorMessages = new List<string>()
            //    {
            //        "no product found"
            //    };
            //    return BadRequest(response);
            //}
            response.httpStatusCode = HttpStatusCode.OK;
            response.Success = true;
            response.Result = products;
            return Ok(response);
        }
        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            APIResponse response = new();
            Product product = await _productService.GetProductById(id);
            if (product is null)
            {
                response.httpStatusCode = HttpStatusCode.NotFound;
                response.Success = false;
                response.ErrorMessages = new List<string>()
                {
                    "no product found"
                };
                return BadRequest(response);
            }
            response.httpStatusCode = HttpStatusCode.OK;
            response.Success = true;
            response.Result = product;
            return Ok(response);
        }
    }
}
