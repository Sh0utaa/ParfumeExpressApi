using Microsoft.AspNetCore.Identity;

namespace ParfumeExpressApi.Interfaces
{
    public interface IUserManagmentRepository
    {
        Task<IdentityUser> FindByEmail(string Email);
        Task<IdentityUser> CreateAdminRole(IdentityUser user);
        Task<bool> ValidateUserCredentialsAsync(string email, string password);
    }
}
