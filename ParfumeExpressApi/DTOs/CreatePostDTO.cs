using ParfumeExpressApi.Models;

namespace ParfumeExpressApi.DTOs
{
    public class createPostDTO
    {
        public double Price { get; set; }
        public string PostTitle { get; set; }
        public string PostBody { get; set; }
        public string? PostImage { get; set; }
        public Gender ParfumeGender { get; set; }
        // <summary>
        // 0 - Unisex
        // 1 - Male
        // 2 - Female
        // </summary>
        public DateTime? PostCreationTime { get; set; }
        public DateTime? PostLastModifiedTime { get; set; }
    }
}
