using System.ComponentModel.DataAnnotations;

namespace Campus_Companion.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(4)]
        public string Password { get; set; } = string.Empty;
    }
}
