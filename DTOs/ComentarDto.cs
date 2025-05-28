namespace Blog.Dtos;

public class ComentarDto
{
    public string Texto { get; set; } = string.Empty;
    public int PostId { get; set; }
    public int UsuarioId { get; set; }
}