using System.ComponentModel.DataAnnotations;

namespace Campus_Companion.Models
{
    public class ProfileViewModel
    {
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Optional: allow user to change password from profile page.
        [DataType(DataType.Password)]
        [MinLength(4)]
        public string? NewPassword { get; set; }
    }
}

