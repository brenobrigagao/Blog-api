using System.ComponentModel.DataAnnotations;

namespace Blog.Dtos;

public class PostDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public string AutorNome { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}
