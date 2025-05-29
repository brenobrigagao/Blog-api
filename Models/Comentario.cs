namespace Blog.Models;

public class Comentario
{
    public int Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public DateTime dataCriacao { get; set; } = DateTime.UtcNow;

    public int PostId { get; set; }
    public Post? Post { get; set; }

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
    public List<ComentarioLike> Likes { get; set; } = new List<ComentarioLike>();
}