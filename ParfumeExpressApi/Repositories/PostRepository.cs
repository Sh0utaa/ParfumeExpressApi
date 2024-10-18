using Microsoft.EntityFrameworkCore;
using ParfumeExpressApi.Data;
using ParfumeExpressApi.DTOs;
using ParfumeExpressApi.Interfaces;
using ParfumeExpressApi.Mappers;
using ParfumeExpressApi.Models;
using System.Reflection.Metadata.Ecma335;

namespace ParfumeExpressApi.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _dataContext;
        private readonly IImageRepository _imageRepository;

        public PostRepository(DataContext dataContext, IImageRepository imageRepository)
        {
            _dataContext = dataContext;
            _imageRepository = imageRepository;
        }

        public async Task<Post> CreateAsync(createPostDTO postModel)
        {
            string? imagePath = await _imageRepository.SaveImageAsync(postModel.PostImagePath);

            var post = postModel.ToPostFromCreatePostDTO(imagePath);


            try
            {
                _dataContext.Posts.AddAsync(post);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

            return post;
        }

        public async Task<Post?> DeleteAsync(int id)
        {
            var postModel = await _dataContext.Posts.FirstOrDefaultAsync(p => p.Id == id);

            await _imageRepository.DeleteImageAsync(postModel.PostImagePath);

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

        public async Task<Post?> GetByTitleAsync(string postTitle)
        {
            return await _dataContext.Posts.FirstOrDefaultAsync(x => x.PostTitle == postTitle);
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
            existingPost.PostImagePath = postModel.PostImagePath;
            existingPost.ParfumeGender = postModel.ParfumeGender;
            existingPost.PostLastModifiedTime = DateTime.Now; // Set modified time to now.

            await _dataContext.SaveChangesAsync();

            return existingPost;
        }

        public async Task<bool> PostExists(int postId)
        {
            return await _dataContext.Posts.AnyAsync(p => p.Id == postId);
        }
    }
}
