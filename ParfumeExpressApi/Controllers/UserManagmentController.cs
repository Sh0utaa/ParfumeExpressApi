using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParfumeExpressApi.Interfaces;
using ParfumeExpressApi.Models;

namespace ParfumeExpressApi.Controllers
{
    [Route("api/userManagment")]
    [ApiController]
    public class UserManagmentController : ControllerBase
    {
        private readonly IUserManagmentRepository _userManagmentRepository;
        public UserManagmentController(IUserManagmentRepository userManagmentRepository)
        {
            _userManagmentRepository = userManagmentRepository;
        }

        [HttpPost("addAdmin/{userEmail}")]
        public async Task<IActionResult> CreateAdmin(string userEmail)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var result = await _userManagmentRepository.CreateAdminRole(userEmail);

            if(result == null) { return BadRequest(ModelState + " Email doesn't exist"); }
            
            else {  return Ok(userEmail + " Added as Admin"); }
        }

        [HttpGet("getUserByEmail/{userEmail}")]
        public async Task<IActionResult> GetUserByEmail(string userEmail)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var result = await _userManagmentRepository.FindByEmail(userEmail);
            
            if(result == null) { return NotFound(userEmail); }
            
            else { return Ok(result);  }
        }
    }
}
