using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParfumeExpressApi.Interfaces;
using ParfumeExpressApi.Models;

namespace ParfumeExpressApi.Controllers
{
    [Route("api/UserManagment")]
    [ApiController]
    public class UserManagmentController : ControllerBase
    {
        private readonly IUserManagmentRepository _userManagmentRepository;
        public UserManagmentController(IUserManagmentRepository userManagmentRepository)
        {
            _userManagmentRepository = userManagmentRepository;
        }

        [HttpPost("{userEmail}")]
        public async Task<IActionResult> CreateAdmin(string userEmail)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            await _userManagmentRepository.CreateAdminRole(userEmail);

            return Ok("Admin role added to " + userEmail);
        }
    }
}
