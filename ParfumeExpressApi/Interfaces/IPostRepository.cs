using ParfumeExpressApi.DTOs;
using ParfumeExpressApi.Models;

namespace ParfumeExpressApi.Interfaces
{
    public interface IPostRepository 
    {
        Task<List<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(int postId);
        Task<Post?> GetByTitleAsync(string postTitle);
        Task<bool> PostExists(int postId);
        Task<Post> CreateAsync(createPostDTO postModel);
        Task<Post?> UpdateAsync(Post postModel);
        Task<Post?> DeleteAsync(int id);
    }
}
