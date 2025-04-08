using BlogApi.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApi.Models.Entities
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public string? Tags { get; set; }  // Almacena los Tags como JSON

        [Required]
        public Category Category { get; set; }
        public DateTime CreatedAt { get; set; }  // Fecha de creación  
        public DateTime UpdatedAt { get; set; }  // Fecha de actualización  

        // Relación con User
        [Required]
        public int UserId { get; set; } // Clave foránea
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        // Propiedad NO mapeada para convertir Tags de JSON a Lista automáticamente
        [NotMapped]
        public List<string> TagsList
        {
            get => string.IsNullOrEmpty(Tags) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(Tags);
            set => Tags = JsonConvert.SerializeObject(value);
        }
    }
}
