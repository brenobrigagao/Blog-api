using Microsoft.EntityFrameworkCore;
using Blog.Models;

namespace Blog.Data
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Role)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
