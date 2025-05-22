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
        .FirstOrDefaultAsync(u => u.Email == login.Email);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.Senha, usuario.SenhaHash)) return Unauthorized("Credenciais inválidas");

        var token = _tokenService.GenerateToken(usuario);
        return Ok(new { token });
    }
    [HttpPost("registrar")]
    public async Task<IActionResult> Registro([FromBody] RegistroDto registro)
    {
        var existe = await _context.Usuarios.AnyAsync(u => u.Email == registro.Email);
        if (existe) return BadRequest("Esse email já está cadastrado!");

        var usuario = new Usuario
        {
            Email = registro.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(registro.Senha),
            Nome = registro.Nome,
            Role = registro.Role
        };
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return Ok(new { message = "O usuário foi cadastrado com sucesso!" });
    }
}
