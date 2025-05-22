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
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Role)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
            .HasOne(a => a.Autor)
            .WithMany(p => p.Posts).
            HasForeignKey(p => p.AutorId);

            modelBuilder.Entity<Comentario>()
            .HasOne(p => p.Post)
            .WithMany(c => c.Comentarios)
            .HasForeignKey(p => p.PostId);

            modelBuilder.Entity<Comentario>()
            .HasOne(u => u.Usuario)
            .WithMany(c => c.Comentarios)
            .HasForeignKey(u => u.UsuarioId);
        }
    }
}
