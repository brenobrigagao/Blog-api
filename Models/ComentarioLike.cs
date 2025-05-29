namespace Blog.Models;

public class ComentarioLike
{
    public int Id { get; set; }
    public int ComentarioId { get; set; }
    public Comentario? Comentario { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
    public DateTime Data { get; set; } = DateTime.UtcNow;
}