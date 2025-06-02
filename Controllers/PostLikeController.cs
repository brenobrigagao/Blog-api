using System.Security.Claims;
using Blog.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.Models;

namespace Blog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostLikeController : ControllerBase
{
    private readonly AppDbContext _context;
    public PostLikeController(AppDbContext context)
    {
        _context = context;
    }
    [HttpPost("{postId}")]
    [Authorize]
    public async Task<IActionResult> CurtirPost(int postId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var jaCurtiu = await _context.PostLikes.AnyAsync(l => l.PostId == postId && l.UsuarioId == userId);
        if (jaCurtiu)
        {
            return BadRequest("Você já curtiu esse post!");
        }
        var novoLike = new PostLike
        {
            PostId = postId,
            UsuarioId = userId,
            Data = DateTime.UtcNow
        };
        _context.PostLikes.Add(novoLike);
        await _context.SaveChangesAsync();

        return Ok("Post curtido com sucesso!");
    }

    [HttpDelete("{postId}")]
    [Authorize]
    public async Task<IActionResult> RetirarCurtida(int postId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var like = await _context.PostLikes.FirstOrDefaultAsync(pl => pl.PostId == postId && pl.UsuarioId == userId);
        if (like == null)
        {
            return NotFound("Like não encontrado");
        }
        _context.PostLikes.Remove(like);
        await _context.SaveChangesAsync();
        return Ok("Like removido com sucesso!");
    }
}