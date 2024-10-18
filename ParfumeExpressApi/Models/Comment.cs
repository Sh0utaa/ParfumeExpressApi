using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParfumeExpressApi.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; } // Store the username for display purposes

        [Required]
        public string Content { get; set; }

        public DateTime CreatedOn { get; private set; } = DateTime.Now;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;

        [Required]
        public string UserId { get; set; }

        [Required]
        public int PostId { get; set; } // Foreign key to Post

        [ForeignKey("PostId")]
        public Post Post { get; set; } // Navigation property
    }
}
