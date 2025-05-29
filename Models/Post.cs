namespace Blog.Models;

public class Post
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public Categoria Categoria { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public int AutorId { get; set; }
    public Usuario? Autor { get; set; }
    public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    public int ComentarioId { get; set; }
    public List<PostLike> Likes { get; set; } = new List<PostLike>();

}