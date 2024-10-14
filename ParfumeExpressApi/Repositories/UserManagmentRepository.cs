using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParfumeExpressApi.Interfaces;

namespace ParfumeExpressApi.Repositories
{
    public class UserManagmentRepository : IUserManagmentRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public UserManagmentRepository(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityUser> CreateAdminRole(string userEmail)
        {
            var userModel = await _userManager.FindByEmailAsync(userEmail);
            if (userModel != null)
            {
                await _userManager.AddToRoleAsync(userModel, "Admin");
            }
            return userModel;
        }

        public async Task<IdentityUser?> FindByEmail(string Email)
        {
            return await _userManager.FindByEmailAsync(Email);
        }

        public async Task<List<IdentityUser>?> GetAllAdminsAsync()
        {
            var admins = new List<IdentityUser>();

            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    admins.Add(user);
                }
            }

            return admins;
        }

        public async Task<List<IdentityUser>?> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<bool> ValidateUserCredentialsAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            return result.Succeeded;
        }

    }
}
