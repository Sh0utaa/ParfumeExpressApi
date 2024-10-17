using ParfumeExpressApi.Models;

namespace ParfumeExpressApi.DTOs
{
    public class UpdatePostDto
    {
        public double? Price { get; set; } // Nullable to allow omission
        public string? PostTitle { get; set; }
        public string? PostBody { get; set; }
        public IFormFile? PostImagePath { get; set; }
        public Gender? ParfumeGender { get; set; }
    }
}
