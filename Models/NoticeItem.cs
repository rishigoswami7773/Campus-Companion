using System.ComponentModel.DataAnnotations;

namespace Campus_Companion.Models
{
    public class NoticeItem
    {
        public int NoticeId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000, MinimumLength = 5)]
        public string Content { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(30)]
        public string Category { get; set; } = "General";
    }
}
