using System.ComponentModel.DataAnnotations;

namespace ParfumeExpressApi.DTOs
{
    public class CreateCommentDto
    {
        [Required]
        public string Content { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        [Required]
        public int PostId { get; set; }
    }
}
