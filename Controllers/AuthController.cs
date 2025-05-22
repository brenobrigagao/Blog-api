using Blog.Data;
using Blog.Dtos;
using Blog.Models;
using Blog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public AuthController(AppDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var usuario = await _context.Usuarios
        .FirstOrDefaultAsync(u => u.Email == login.Email && u.Senha == login.Senha);

        if (usuario == null) return Unauthorized("Credenciais inválidas");
        var token = _tokenService.GenerateToken(usuario);
        return Ok(new { token });
    }
}
