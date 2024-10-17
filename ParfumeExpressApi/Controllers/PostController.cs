using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ParfumeExpressApi.DTOs;
using ParfumeExpressApi.Interfaces;
using ParfumeExpressApi.Mappers;
using ParfumeExpressApi.Models;

namespace ParfumeExpressApi.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var posts = await _postRepository.GetAllAsync();

            if (posts == null || !posts.Any())
            {
                return NotFound("No posts found.");
            }

            return Ok(posts);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = await _postRepository.GetByIdAsync(id);

            if (post == null)
            {
                return NotFound("No posts found.");
            }

            return Ok(post);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] createPostDTO postDto)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                await _postRepository.CreateAsync(postDto);

                return Ok("Post Successfully created!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePartial(int id, [FromBody] UpdatePostDto updatePostDto)
        {
            var existingPost = await _postRepository.GetByIdAsync(id);

            if (existingPost == null) { return NotFound("Post with the Id of " + id + " Does not exist!"); }

            // Update only the fields that are provided.
            if (updatePostDto.Price.HasValue)
            {
                existingPost.Price = updatePostDto.Price.Value;
            }

            if (!string.IsNullOrEmpty(updatePostDto.PostTitle))
            {
                existingPost.PostTitle = updatePostDto.PostTitle;
            }

            if (!string.IsNullOrEmpty(updatePostDto.PostBody))
            {
                existingPost.PostBody = updatePostDto.PostBody;
            }

            if (!string.IsNullOrEmpty(updatePostDto.PostImage))
            {
                existingPost.PostImagePath = updatePostDto.PostImage;
            }

            if (updatePostDto.ParfumeGender.HasValue)
            {
                existingPost.ParfumeGender = updatePostDto.ParfumeGender.Value;
            }

            existingPost.PostLastModifiedTime = DateTime.Now; // Update modified time

            await _postRepository.UpdateAsync(existingPost); // Make sure to implement this method.

            return Ok("Post Updated Successfully");

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var responseModel = await _postRepository.DeleteAsync(id);

            if(responseModel == null) { return NotFound("Post not found");  }

            return Ok("Post deleted successfully");
        }
    }
}
