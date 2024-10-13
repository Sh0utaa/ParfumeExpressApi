using Microsoft.EntityFrameworkCore;
using ParfumeExpressApi.Data;
using ParfumeExpressApi.Interfaces;
using ParfumeExpressApi.Models;
using System.Reflection.Metadata.Ecma335;

namespace ParfumeExpressApi.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _dataContext;
        public PostRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Post> CreateAsync(Post postModel)
        {
            await _dataContext.AddAsync(postModel);
            await _dataContext.SaveChangesAsync();

            return postModel;
        }

        public async Task<Post?> DeleteAsync(int id)
        {
            var postModel = await _dataContext.Posts.FirstOrDefaultAsync(p => p.Id == id);

            if (postModel == null) { return null; }

            _dataContext.Posts.Remove(postModel);
            await _dataContext.SaveChangesAsync();
            return postModel;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            return await _dataContext.Posts.ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int postId)
        {
            return await _dataContext.Posts.FindAsync(postId);
        }

        public async Task<Post?> UpdateAsync(Post postModel)
        {
            var existingPost = await _dataContext.Posts.FirstOrDefaultAsync(x => x.Id == postModel.Id);

            if (existingPost == null)
            {
                return null; // Return null if the post doesn't exist.
            }

            // Update only the fields that are allowed to change.
            existingPost.Price = postModel.Price;
            existingPost.PostTitle = postModel.PostTitle;
            existingPost.PostBody = postModel.PostBody;
            existingPost.PostImage = postModel.PostImage;
            existingPost.ParfumeGender = postModel.ParfumeGender;
            existingPost.PostLastModifiedTime = DateTime.Now; // Set modified time to now.

            await _dataContext.SaveChangesAsync();

            return existingPost;
        }

    }
}
