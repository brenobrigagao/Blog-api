using Blog.Data;
using Blog.Models;
using Blog.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
[Route("[controller]")]
public class ComentarioController : ControllerBase
{
    private readonly AppDbContext _context;
    public ComentarioController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet("listar-comentarios")]
    public async Task<IActionResult> ListarTodos()
    {
        var comentarios = await _context.Comentarios
        .Include(c => c.Post)
        .Include(c => c.Usuario)
        .Select(p => new ComentarioDto
        {
            Id = p.Id,
            Texto = p.Texto,
            DataCriacao = p.dataCriacao,
            AutorNome = p.Usuario.Nome,
            PostNome = p.Post.Titulo
        }).ToListAsync();
        return Ok(comentarios);
    }
    [HttpPost("fazer-comentario")]
    public async Task<IActionResult> Comentar([FromBody] ComentarDto dto)
    {
        var post = await _context.Posts.FindAsync(dto.PostId);
        var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
        if (post == null || usuario == null)
        {
            return NotFound("Usuario ou post não encontrado");
        }
        var comentario = new Comentario
        {
            Texto = dto.Texto,
            PostId = dto.PostId,
            UsuarioId = dto.UsuarioId,
            dataCriacao = DateTime.UtcNow
        };
        _context.Comentarios.Add(comentario);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Comentario criado com sucesso!", comentario.Id });
    

    }
}