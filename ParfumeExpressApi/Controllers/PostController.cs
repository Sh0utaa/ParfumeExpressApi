using Microsoft.AspNetCore.Mvc;
using ParfumeExpressApi.Interfaces;

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
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }

            var Posts = await _postRepository.GetAllAsync();

            return Ok(Posts);
        }

    }
}
