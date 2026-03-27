using System.ComponentModel.DataAnnotations;

namespace Campus_Companion.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(4)]
        public string Password { get; set; } = string.Empty;
    }
}

