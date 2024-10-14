using Microsoft.AspNetCore.Identity;

namespace ParfumeExpressApi.Interfaces
{
    public interface IUserManagmentRepository
    {
        Task<IdentityUser?> FindByEmail(string Email);
        Task<List<IdentityUser>?> GetAllUsersAsync();
        Task<List<IdentityUser>?> GetAllAdminsAsync();
        Task<IdentityUser> CreateAdminRole(string userEmail);
        Task<bool> ValidateUserCredentialsAsync(string email, string password);
    }
}
