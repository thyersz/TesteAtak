using System.ComponentModel.DataAnnotations;

namespace TesteAtak.Models
{
    public class RegisterModel
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Phone]
        public string? Telefone { get; set; }
    }
}
