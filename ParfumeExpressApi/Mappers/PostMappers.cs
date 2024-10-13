using ParfumeExpressApi.DTOs;
using ParfumeExpressApi.Models;

namespace ParfumeExpressApi.Mappers
{
    public static class PostMappers
    {
        public static Post ToPostFromCreatePostDTO(this createPostDTO postDto)
        {
            return new Post
            {
                Price = postDto.Price,
                PostTitle = postDto.PostTitle,
                PostBody = postDto.PostBody,
                PostImage = postDto.PostImage,
                ParfumeGender = postDto.ParfumeGender,
                //PostCreationTime = postDto.PostCreationTime,
                PostLastModifiedTime = postDto.PostLastModifiedTime
            };
        }
    }
}
