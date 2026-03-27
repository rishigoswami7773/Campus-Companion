using System.ComponentModel.DataAnnotations;

namespace Campus_Companion.Models
{
    public class ScheduleItem
    {
        public int ScheduleId { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(20)]
        public string Time { get; set; } = string.Empty;

        [Required]
        [StringLength(60)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Category { get; set; } = "Lecture";
    }
}
