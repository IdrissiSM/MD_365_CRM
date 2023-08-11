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
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private APIResponse _response;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
            _response = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllAccounts()
        {
            List<Models.Account> accounts = await _accountService.GetAllAccountsAsync();

            _response.httpStatusCode = HttpStatusCode.OK;
            _response.Result = accounts;

            return Ok(_response);
            
        }

        [HttpGet("GetAllAccountsOrderedByRevenue")]
        public async Task<ActionResult<APIResponse>> GetAllAccountsOrderedByRevenue()
        {
            List<Account> accounts = await _accountService.GetAllAcountsOrderedByRevenueAsync();

            _response.httpStatusCode = HttpStatusCode.OK;
            _response.Result = accounts;

            return Ok(_response);
        }
    }
}
