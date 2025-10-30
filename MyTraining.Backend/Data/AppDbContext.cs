using Microsoft.EntityFrameworkCore;
using MyTraining.Backend.Models;


namespace MyTraining.Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Trainer> Trainers => Set<Trainer>();
        public DbSet<Course> Courses => Set<Course>();

    }
}
