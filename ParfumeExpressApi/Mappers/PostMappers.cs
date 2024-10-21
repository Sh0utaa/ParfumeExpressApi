using ParfumeExpressApi.DTOs;
using ParfumeExpressApi.Models;

namespace ParfumeExpressApi.Mappers
{
    public static class PostMappers
    {
        public static Post ToPostFromCreatePostDTO(this createPostDTO postDto, string? imagePath)
        {
            return new Post
            {
                Price = postDto.Price,
                PostBrand = postDto.PostBrand,
                PostTitle = postDto.PostTitle,
                PostBody = postDto.PostBody,
                PostImagePath = imagePath,
                ParfumeGender = postDto.ParfumeGender,
                //PostCreationTime = postDto.PostCreationTime,
                PostLastModifiedTime = postDto.PostLastModifiedTime
            };
        }
    }
}
