namespace Blog.Dtos;

using System.ComponentModel.DataAnnotations;
using Blog.Models.Enums;


public class RegistroDto
{
    [Required]
    public string Nome { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Senha { get; set; } = string.Empty;
    [Required]
    public Role Role { get; set; }
}