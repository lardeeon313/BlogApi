using BlogApi.Models.Enums;

namespace BlogApi.Models.Dtos
{
    public class PostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Category Category { get; set; }
        public List<string> Tags { get; set; }
    }
}
