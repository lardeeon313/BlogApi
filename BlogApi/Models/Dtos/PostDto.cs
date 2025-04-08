using BlogApi.Models.Enums;

namespace BlogApi.Models.Dtos
{
    public class PostDto
    {
        public int Id { get; set; } 
        public string Title { get; set; } = string.Empty; // Evita null en Title
        public string Content { get; set; } = string.Empty; // Evita null en Content
        public Category Category { get; set; }
        public List<string> Tags { get; set; } = new List<string>(); // Evita null en Tags
    }
}
