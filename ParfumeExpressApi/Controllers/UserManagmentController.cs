using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParfumeExpressApi.DTOs;
using ParfumeExpressApi.Interfaces;
using System.IdentityModel.Tokens.Jwt;

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

            if (result == null) { return BadRequest(ModelState + " Email doesn't exist"); }

            else { return Ok(userEmail + " Added as Admin"); }
        }

        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var result = await _userManagmentRepository.GetAllUsersAsync();

            if (result == null) { return NotFound(); }

            return Ok(result);
        }

        [HttpGet("getAllAdmins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var result = await _userManagmentRepository.GetAllAdminsAsync();
             
            if (result == null) { return NotFound(); };
            
            return Ok(result);
        }

        [HttpGet("getUserByEmail/{userEmail}")]
        public async Task<IActionResult> GetUserByEmail(string userEmail)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var result = await _userManagmentRepository.FindByEmail(userEmail);

            if (result == null) { return NotFound(userEmail); }

            return Ok(result);
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (email == null)
            {
                return Unauthorized(new { message = "User email not found in token" });
            }
            
            var user = await _userManagmentRepository.FindByEmail(email);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Change the password
            var result = await _userManagmentRepository.ChangePasswordAsync(email, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new { errors = result.Errors });
            }


            return Ok(new { message = "Password changed successfully "});
        }


    }
}
