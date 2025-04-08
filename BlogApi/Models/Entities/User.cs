using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty; // Se almacenará la contraseña encriptada

        [Required]
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; }

        // Relación con Post (Un usuario tiene muchos posts)
        public ICollection<Post> Posts { get; set; } = [];
    }
}
