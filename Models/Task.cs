namespace Campus_Companion.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }

        // Added to fix CS1061: reference from Views\TaskAdmin\Index.cshtml to item.Priority
        public string Priority { get; set; }
    }
}
