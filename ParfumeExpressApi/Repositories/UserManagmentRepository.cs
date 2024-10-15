using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParfumeExpressApi.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ParfumeExpressApi.Repositories
{
    public class UserManagmentRepository : IUserManagmentRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IPasswordValidator<IdentityUser> _passwordValidator;

        public UserManagmentRepository(
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            SignInManager<IdentityUser> signInManager,
            IPasswordValidator<IdentityUser> passwordValidator
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _passwordValidator = passwordValidator;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            // Find the user by email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            var isCurrentPasswordValid = await _userManager.CheckPasswordAsync(user, currentPassword);
            if (!isCurrentPasswordValid)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Current password is incorrect." });
            }

            var passwordValidationResult = await _passwordValidator.ValidateAsync(_userManager, user, newPassword);
            if (!passwordValidationResult.Succeeded)
            {
                var errors = string.Join(", ", passwordValidationResult.Errors.Select(e => e.Description));
                return IdentityResult.Failed(new IdentityError { Description = errors });
            }

            // Attempt to change the password
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
            {
                return IdentityResult.Success;
            }

            return result;
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
