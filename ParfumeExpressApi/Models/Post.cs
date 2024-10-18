namespace ParfumeExpressApi.Models
{
    public class Post
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string PostTitle { get; set; }
        public string PostBody { get; set; }
        public string? PostImagePath { get; set; }
        public Gender ParfumeGender { get; set; }

        /// <summary>
        /// 0 - Unisex, 1 - Male, 2 - Female
        /// </summary>
        public DateTime PostCreationTime { get; private set; } = DateTime.Now; // Set only once during creation.
        public DateTime? PostLastModifiedTime { get; set; } // Updated when modified.

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
