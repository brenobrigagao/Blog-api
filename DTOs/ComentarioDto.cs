namespace Blog.Dtos;

public class ComentarioDto
{
    public int Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public string AutorNome { get; set; } = string.Empty;
    public string PostNome { get; set; } = string.Empty;
    public int QuantidadeLikes { get; set; }
}