using ParfumeExpressApi.Models;

namespace ParfumeExpressApi.Interfaces
{
    public interface IPostRepository 
    {
        Task<List<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(int postId);
        Task<Post> CreateAsync(Post postModel);
        Task<Post?> UpdateAsync(Post postModel);
        Task<Post?> DeleteAsync(int id);
    }
}
