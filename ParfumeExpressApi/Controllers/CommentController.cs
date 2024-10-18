using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ParfumeExpressApi.DTOs;
using ParfumeExpressApi.Interfaces;
using ParfumeExpressApi.Models;
using System.Security.Claims;

namespace ParfumeExpressApi.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;

        public CommentController(ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }

        [HttpGet("/getAllComments")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid) { return BadRequest("ModelState Not Valid"); }

            var allComments = await _commentRepository.GetAllComments();

            if (allComments == null || allComments.Count == 0) { return NotFound("No comments were found"); }

            return Ok(allComments);
        }

        [HttpGet("/getByUserName/{userName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByUserName(string userName)
        {
            if (!ModelState.IsValid) { return BadRequest("ModelState Not Valid"); }

            var allComments = await _commentRepository.GetCommentsByUserName(userName);

            if (allComments == null || allComments.Count == 0) { return NotFound($"No comment with the username {userName} was found ");  }
            
            return Ok(allComments);
        }

        [HttpPost("/createComment")]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            var userName = User.Identity.Name;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("User not identified.");


            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not identified.");

            if (await _postRepository.PostExists(commentDto.PostId) == false)
            {
                return NotFound($"Post {commentDto.PostId} doesn't exist");
            }

            Comment commentModel = new Comment()
            {
                UserName = userName,
                Content = commentDto.Content,
                UpdatedOn = commentDto.UpdatedOn,
                UserId = userId,
                PostId = commentDto.PostId,
            };
            
            var result = await _commentRepository.CreateComment(commentModel);

            if (result.Succeeded) { return Ok("Comment added successfully!");  }
            else { return BadRequest(); }
        }
    }
}
