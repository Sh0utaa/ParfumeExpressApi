using Microsoft.AspNetCore.Identity;
using ParfumeExpressApi.Models;

namespace ParfumeExpressApi.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllComments();
        Task<List<Comment>> GetCommentsByUserName(string userName);
        Task<List<Comment>> GetCommentsByPostId(int postId);
        Task<IdentityResult> CreateComment(Comment comment);
    }
}
