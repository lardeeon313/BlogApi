using BlogApi.Models.Enums;
using System.ComponentModel;

namespace BlogApi.Models.Entities
{
    public class Post
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Category Category { get; set; }
        public List<string> Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }






    }
}
