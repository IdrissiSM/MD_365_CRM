using MD_365_CRM.Models;
using MD_365_CRM.Responses;
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
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            APIResponse response = new();
            List<User> users = await _usersService.GetUsersAsync();
            if (users is null)
            {
                response.httpStatusCode = HttpStatusCode.NotFound;
                response.Success = false;
                response.ErrorMessages = new List<string>()
                {
                    "no users found"
                };
                return BadRequest(response);
            }
            response.httpStatusCode = HttpStatusCode.OK;
            response.Success = true;
            response.Result = users;
            return Ok(response);
        }
        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId) 
        {
            APIResponse response = new();
            bool isDeleted = await _usersService.DeleteUserAsync(userId);
            if (!isDeleted)
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
            response.Result = "Deleted succcessfuly";
            return Ok(response);
        }
        [HttpDelete("DeleteUserByEmail/{email}")]
        public async Task<IActionResult> DeleteUserByEmail(string email)
        {
            APIResponse response = new();
            bool isDeleted = await _usersService.DeleteUserByEmailAsync(email);
            if (!isDeleted)
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
            response.Result = "Deleted succcessfuly";
            return Ok(response);
        }
    }
}
