public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime DueDate { get; set; }
    public string Priority { get; set; } // Add this property to fix CS1061
}
