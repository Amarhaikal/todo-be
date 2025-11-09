using FirstApi.Model;
using Microsoft.EntityFrameworkCore;

namespace FirstApi
{
    public class FirstApiContext: DbContext
    {
        public FirstApiContext(DbContextOptions<FirstApiContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LevelPriority>().HasIndex(l => l.Code).IsUnique();

            modelBuilder.Entity<LevelPriority>().HasData(
               new LevelPriority { Id = 1, Code = "L", Name = "Low" },
               new LevelPriority { Id = 2, Code = "M", Name = "Medium" },
               new LevelPriority { Id = 3, Code = "H", Name = "High" }
            );
        }

        public DbSet<LevelPriority> LevelPriorities { get; set; }
        public DbSet<Todo> Todos { get; set; }
    }
}
