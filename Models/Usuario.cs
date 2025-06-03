using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blog.Models.Enums;

namespace Blog.Models;

public class Usuario
{
    public int Id { get; set; }
    [Required]
    public string Nome { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string SenhaHash { get; set; } = string.Empty;
    [Required]
    public Role Role { get; set; } = Role.Leitor;

    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    public List<PostLike> PostLikes { get; set; } = new List<PostLike>();
    public List<ComentarioLike> ComentarioLikes { get; set; } = new List<ComentarioLike>();
    public ICollection<Seguidor>? Seguindo { get; set; }
    public ICollection<Seguidor>? Seguidores { get; set; }

    

}
