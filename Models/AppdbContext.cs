using Campus_Companion.Data;
using Campus_Companion.Models;
using Microsoft.EntityFrameworkCore;
namespace Campus_companion_Hackathon.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Teacher> Teachers { get; set; }
    }
}
