namespace Blog.Models;

public class Seguidor
{
    public int UsuarioSeguidorId { get; set; }
    public Usuario? UsuarioSeguidor { get; set; }
    public int UsuarioSeguidoId { get; set; }
    public Usuario? UsuarioSeguido { get; set; }
}