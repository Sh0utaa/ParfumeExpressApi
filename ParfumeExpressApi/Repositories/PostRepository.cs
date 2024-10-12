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

            if (postModel != null) { return null; }

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

        public async Task<Post?> UpdateAsync(int id, Post postModel)
        {
            var existingPostModel = await _dataContext.Posts.FindAsync(id);

            if (existingPostModel == null)
            {
                return null;
            }

            postModel.Price = existingPostModel.Price;
            postModel.PostTitle = existingPostModel.PostTitle;
            postModel.PostBody = existingPostModel.PostTitle;
            postModel.PostImage = existingPostModel.PostImage;
            postModel.ParfumeGender = existingPostModel.ParfumeGender;
            postModel.PostCreationTime = existingPostModel.PostCreationTime;
            postModel.PostLastModifiedTime = existingPostModel.PostLastModifiedTime;

            await _dataContext.SaveChangesAsync();

            return postModel;
        }
    }
}
