using Campus_Companion.Models;
//using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Campus_Companion.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

        // Tables (Models)
        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<TaskItem> Tasks { get; set; }
    }
}