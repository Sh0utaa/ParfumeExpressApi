using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParfumeExpressApi.Data;
using ParfumeExpressApi.Interfaces;
using ParfumeExpressApi.Models;

namespace ParfumeExpressApi.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;

        public CommentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetAllComments()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<List<Comment>> GetCommentsByUserName(string userName)
        {
            var allComments = _context.Comments.Where(x => x.UserName == userName);
            return await allComments.ToListAsync();
        }

        public async Task<IdentityResult> CreateComment(Comment comment)
        {
            try
            {
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Creating comment error: {ex}" });
            }

        }

        public async Task<List<Comment>> GetCommentsByPostId(int postId)
        {
            var allComments = _context.Comments.Where(x => x.PostId == postId);
            return await allComments.ToListAsync();
        }
    }
}
