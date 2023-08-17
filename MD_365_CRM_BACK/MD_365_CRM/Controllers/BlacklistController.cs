using Azure;
using MD_365_CRM.Models;
using MD_365_CRM.Requests;
using MD_365_CRM.Responses;
using MD_365_CRM.Services;
using MD_365_CRM.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace MD_365_CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlacklistController : ControllerBase
    {
        private readonly IBlacklistedUserService _blacklistedUserService;

        public BlacklistController(IBlacklistedUserService blacklistedUserService)
        {
            _blacklistedUserService = blacklistedUserService;
        }
        [HttpGet("GetBlacklistedUsers")]
        public async Task<IActionResult> GetBlacklistedUsers()
        {
            APIResponse response = new();
            List<BlacklistedUser> blacklistedUsers = await _blacklistedUserService.GetBlacklistedUsersAsync();
            if (blacklistedUsers is null)
            {
                response.httpStatusCode = HttpStatusCode.NotFound;
                response.Success = false;
                response.ErrorMessages = new List<string>()
                {
                    "no blacklisted users found"
                };
                return BadRequest(response);
            }
            response.httpStatusCode = HttpStatusCode.OK;
            response.Success = true;
            response.Result = blacklistedUsers;
            return Ok(response);
        }
        [HttpPost("AddToBlacklist")]
        public async Task<IActionResult> AddUserToBlacklist([FromBody] BlacklistedUserRequest request)
        {
            APIResponse response = new();
            bool isAdded = await _blacklistedUserService.AddUserToBlacklist(request.Email);
            if (!isAdded)
            {
                response.httpStatusCode = HttpStatusCode.NotFound;
                response.Success = false;
                response.ErrorMessages = new List<string>()
                {
                    "something went wrong"
                };
                return BadRequest(response);
            }
            response.httpStatusCode = HttpStatusCode.OK;
            response.Success = true;
            response.Result = "User added to blacklist.";
            return Ok(response);
        }
        [HttpDelete("RemoveFromBlacklist/{email}")]
        public async Task<IActionResult> RemoveUserFromBlacklist(string email)
        {
            APIResponse response = new();
            bool isRemoved = await _blacklistedUserService.RemoveUserFromBlacklist(email);
            if (!isRemoved)
            {
                response.httpStatusCode = HttpStatusCode.NotFound;
                response.Success = false;
                response.ErrorMessages = new List<string>()
                {
                    "something went wrong"
                };
                return BadRequest(response);
            }
            response.httpStatusCode = HttpStatusCode.OK;
            response.Success = true;
            response.Result = "User removed from blacklist.";
            return Ok(response);
        }
        [HttpGet("CheckBlacklist/{email}")]
        public async Task<IActionResult> CheckBlacklist(string email)
        {
            bool isBlacklisted = await _blacklistedUserService.IsUserBlacklisted(email);
            return Ok(new { IsBlacklisted = isBlacklisted });
        }
    }
}
