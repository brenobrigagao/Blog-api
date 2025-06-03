using System.ComponentModel.DataAnnotations;
using Blog.Models.Enums;
namespace Blog.Dtos;
public class CriarPostDto
{
    [Required]
    public string Titulo { get; set; } = string.Empty;
    [Required]
    public string Conteudo { get; set; } = string.Empty;
    public Categoria Categoria { get; set; }
}