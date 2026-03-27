using System.ComponentModel.DataAnnotations;

namespace Campus_Companion.Models
{
    public enum TaskPriority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public enum ItemStatus
    {
        Pending = 0,
        Completed = 1
    }

    public class TaskItem
    {
        public int TaskId { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public ItemStatus Status { get; set; } = ItemStatus.Pending;
    }
}
