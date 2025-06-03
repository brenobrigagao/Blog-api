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
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<ComentarioLike> ComentarioLikes{ get; set; }
        public DbSet<Seguidor> Seguidores { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<PostLike>()
            .HasIndex(pl => new { pl.PostId, pl.UsuarioId })
            .IsUnique();

            modelBuilder.Entity<ComentarioLike>()
            .HasIndex(cl => new { cl.ComentarioId, cl.UsuarioId })
            .IsUnique();

            modelBuilder.Entity<Seguidor>()
            .HasKey(s => new { s.SeguidorId, s.SeguidoId });

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
            .HasOne(a => a.Autor)
            .WithMany(p => p.Posts)
            .HasForeignKey(p => p.AutorId);

            modelBuilder.Entity<Comentario>()
            .HasOne(p => p.Post)
            .WithMany(c => c.Comentarios)
            .HasForeignKey(p => p.PostId);

            modelBuilder.Entity<Comentario>()
            .HasOne(u => u.Usuario)
            .WithMany(c => c.Comentarios)
            .HasForeignKey(u => u.UsuarioId);

            modelBuilder.Entity<PostLike>()
            .HasOne(pl => pl.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(pl => pl.PostId);

            modelBuilder.Entity<PostLike>()
            .HasOne(pl => pl.Usuario)
            .WithMany(u => u.PostLikes)
            .HasForeignKey(pl => pl.UsuarioId);

            modelBuilder.Entity<ComentarioLike>()
            .HasOne(cl => cl.Comentario)
            .WithMany(c => c.Likes)
            .HasForeignKey(cl => cl.ComentarioId);

            modelBuilder.Entity<Seguidor>()
            .HasOne(s => s.UsuarioQueSegue)
            .WithMany(u => u.Seguindo)
            .HasForeignKey(s => s.SeguidorId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Seguidor>()
            .HasOne(s => s.UsuarioSendoSeguido)
            .WithMany(u => u.Seguidores)
            .HasForeignKey(s => s.SeguidoId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
