using Microsoft.AspNetCore.Mvc;
using Blog.Data;
using System.Security.Claims;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly AppDbContext _context;
    public UsuarioController(AppDbContext context)
    {
        _context = context;
    }
    [HttpPost("{id}/seguir")]
    [Authorize]
    public async Task<IActionResult> Seguir(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        if (userId == id) return BadRequest("Você não pode seguir a si mesmo");
        var jaSegue = await _context.Seguidores.FindAsync(userId, id);
        if (jaSegue != null) return BadRequest("Você já está seguindo esse usuário!");

        var seguir = new Seguidor
        {
            UsuarioSeguidorId = userId,
            UsuarioSeguidoId = id
        };

        _context.Seguidores.Add(seguir);
        await _context.SaveChangesAsync();
        return Ok("Seguindo com sucesso!");
    }

    [HttpDelete("{id}/deixar-seguir")]
    [Authorize]
    public async Task<IActionResult> DeixarDeSeguir(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var relacao = await _context.Seguidores.FindAsync(userId, id);
        if (relacao == null) return BadRequest("Você não segue esse usuário!");

        _context.Seguidores.Remove(relacao);
        await _context.SaveChangesAsync();
        return Ok("Você deixou de seguir!");
    }
    [HttpGet("{id}/ver-seguidos")]
    [Authorize]
    public async Task<IActionResult> VerSeguidos(int id)
    {
        var seguindo = await _context.Seguidores
        .Where(s => s.UsuarioSeguidorId == id)
        .Include(s => s.UsuarioSeguido)
        .Select(s => new
        {
            s.UsuarioSeguidoId,
            Nome = s.UsuarioSeguido!.Nome
        }).ToListAsync();

        return Ok(seguindo);
    }

    [HttpGet("{id}/ver-serguidores")]
    [Authorize]
    public async Task<IActionResult> VerSeguidores(int id)
    {
        var seguidor = await _context.Seguidores
        .Where(s => s.UsuarioSeguidoId == id)
        .Include(s => s.UsuarioSeguidor)
        .Select(s => new
        {
            s.UsuarioSeguidorId,
            Nome = s.UsuarioSeguidor!.Nome
        }).ToListAsync();
        return Ok(seguidor);
    }

}