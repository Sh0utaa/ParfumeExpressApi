using Microsoft.AspNetCore.Identity;

namespace ParfumeExpressApi.Interfaces
{
    public interface IUserManagmentRepository
    {
        Task<IdentityUser> FindByEmail(string Email);
        Task<IdentityUser> CreateAdminRole(string userEmail);
        Task<bool> ValidateUserCredentialsAsync(string email, string password);
    }
}
