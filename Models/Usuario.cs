using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public string Senha { get; set; } = string.Empty;
    [Required]
    public Role Role { get; set; } = Role.Leitor;

    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    

}
