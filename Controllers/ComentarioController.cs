using Blog.Data;
using Blog.Models;
using Blog.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
    [Authorize]
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
    [Authorize]
    [HttpPut("editar-comentario/{id}")]
    public async Task<IActionResult> Editar(int id, [FromBody] EditarComentarioDto dto)
    {
        var comentario = await _context.Comentarios.FindAsync(id);
        if (comentario == null) return NotFound("Esse comentário não existe!");
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if (comentario.UsuarioId != usuarioId) return Forbid("Você não tem permissão para editar esse comentario.");
        comentario.Texto = dto.Texto;
        await _context.SaveChangesAsync();
        return Ok(new { message = "Comentario editado com sucesso" });
    }
    [Authorize]
    [HttpDelete("excluir-comentario/{id}")]
    public async Task<IActionResult> ApagarComentario(int id)
    {
        var comentario = await _context.Comentarios.FindAsync(id);
        if (comentario == null) return NotFound("O comentario não existe!");
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if (comentario.UsuarioId != usuarioId) return Forbid("Você não tem permissão de apagar esse comentario.");
        _context.Comentarios.Remove(comentario);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Comentario apagado com sucesso!" });
    }
    [HttpGet("comentarios-post/{PostId}")]
    public async Task<IActionResult> ComentariosDoPost(int PostId)
    {
        var comentarios = await _context.Comentarios
        .Where(c => c.PostId == PostId)
        .Include(c => c.Usuario)
        .Include(c => c.Post)
        .Select(c => new ComentarioDto
        {
            Id = c.Id,
            Texto = c.Texto,
            DataCriacao = c.dataCriacao,
            AutorNome = c.Usuario.Nome,
            PostNome = c.Post.Titulo
        }).ToListAsync();
        return Ok(comentarios);
    }
}