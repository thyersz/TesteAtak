using System.ComponentModel.DataAnnotations;

namespace TesteAtak.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? Telefone { get; set; }

        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
