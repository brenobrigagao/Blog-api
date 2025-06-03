namespace Blog.Models;

public class Seguidor
{
    public int SeguidorId { get; set; }
    public int SeguidoId { get; set; }
    public Usuario? UsuarioSeguidor { get; set; }
    public Usuario? UsuarioSeguido { get; set; }
}