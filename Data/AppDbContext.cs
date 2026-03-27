using Campus_Companion.Models;
using Microsoft.EntityFrameworkCore;

namespace Campus_Companion.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Student> Students => Set<Student>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<ScheduleItem> Schedules => Set<ScheduleItem>();
        public DbSet<NoticeItem> Notices => Set<NoticeItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // EF Core convention can miss non-standard "<EntityName>Id" patterns.
            // Explicitly define PKs for all exam entities.
            modelBuilder.Entity<TaskItem>().HasKey(t => t.TaskId);
            modelBuilder.Entity<ScheduleItem>().HasKey(s => s.ScheduleId);
            modelBuilder.Entity<NoticeItem>().HasKey(n => n.NoticeId);
        }
    }
}
