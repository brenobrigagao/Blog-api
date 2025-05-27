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
}