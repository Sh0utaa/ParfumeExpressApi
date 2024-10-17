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
        public PostRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Post> CreateAsync(createPostDTO postModel)
        {
            string? imagePath = await SaveImageAsync(postModel.PostImagePath);

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

        private async Task<string?> SaveImageAsync(IFormFile? image)
        {
            if (image == null || image.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

            var fileName = $"{Guid.NewGuid()}_{image.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/images/{fileName}"; // Return relative path
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
            existingPost.PostImagePath = postModel.PostImagePath;
            existingPost.ParfumeGender = postModel.ParfumeGender;
            existingPost.PostLastModifiedTime = DateTime.Now; // Set modified time to now.

            await _dataContext.SaveChangesAsync();

            return existingPost;
        }

    }
}
