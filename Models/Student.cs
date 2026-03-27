using System.ComponentModel.DataAnnotations;

namespace Campus_Companion.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        public string Course { get; set; }

    }
}
