using System.Security.Claims;
using Blog.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.Models;

namespace Blog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComentarioLikeController : ControllerBase
{
    private readonly AppDbContext _context;
    public ComentarioLikeController(AppDbContext context)
    {
        _context = context;
    }
    [HttpPost("{comentarioId}")]
    [Authorize]
    public async Task<IActionResult> CurtirComentario(int comentarioId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var jaCurtiu = await _context.ComentarioLikes.AnyAsync(l => l.ComentarioId == comentarioId && l.UsuarioId == userId);
        if (jaCurtiu)
        {
            return BadRequest("Você já curtiu esse comentario!");
        }
        var novoLike = new ComentarioLike
        {
            ComentarioId = comentarioId,
            UsuarioId = userId,
            Data = DateTime.UtcNow
        };
        _context.ComentarioLikes.Add(novoLike);
        await _context.SaveChangesAsync();

        return Ok("Comentario curtido com sucesso!");
    }

    [HttpDelete("{comentarioId}")]
    [Authorize]
    public async Task<IActionResult> RetirarCurtida(int comentarioId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var like = await _context.ComentarioLikes.FirstOrDefaultAsync(pl => pl.ComentarioId == comentarioId && pl.UsuarioId == userId);
        if (like == null)
        {
            return NotFound("Like não encontrado");
        }
        _context.ComentarioLikes.Remove(like);
        await _context.SaveChangesAsync();
        return Ok("Like removido com sucesso!");
    }
}