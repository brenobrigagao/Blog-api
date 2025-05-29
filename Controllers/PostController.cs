using System.Security.Claims;
using Blog.Data;
using Blog.Dtos;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly AppDbContext _context;
    public PostController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("listar-posts")]
    public async Task<IActionResult> Listar([FromQuery] string? categoria)
    {
        var query = _context.Posts.Include(p => p.Autor).AsQueryable();
        if (!string.IsNullOrEmpty(categoria) &&
        Enum.TryParse<Categoria>(categoria, ignoreCase: true, out var categoriaEnum))
        {
            query = query.Where(p => p.Categoria == categoriaEnum);
        }
        var posts = await query
        .Select(p => new PostDTO
        {
            Id = p.Id,
            Titulo = p.Titulo,
            Conteudo = p.Conteudo,
            AutorNome = p.Autor.Nome,
            DataCriacao = p.DataCriacao

        }).ToListAsync();
        return Ok(posts);
    }
    [Authorize(Roles = "Autor")]
    [HttpPost("criar-post")]
    public async Task<IActionResult> CriarPost([FromBody] CriarPostDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var post = new Post
        {
            Titulo = dto.Titulo,
            Conteudo = dto.Conteudo,
            AutorId = userId,
            DataCriacao = DateTime.UtcNow,
            Categoria = dto.Categoria
        };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Post criado com sucesso!", post.Id });
    }
    [Authorize(Roles = "Autor")]
    [HttpPut("editar-post/{id}")]
    public async Task<IActionResult> EditarPost(int id, [FromBody] CriarPostDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var post = await _context.Posts.FindAsync(id);

        if (post == null || post.AutorId != userId) return Unauthorized("Você não pode editar esse post!");
        post.Titulo = dto.Titulo;
        post.Conteudo = dto.Conteudo;
        await _context.SaveChangesAsync();
        return Ok("Post atualizado com sucesso!");
    }

    [Authorize(Roles = "Autor")]
    [HttpDelete("deletar-post/{id}")]
    public async Task<IActionResult> DeletarPost(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var post = await _context.Posts.FindAsync(id);

        if (post == null || post.AutorId != userId) return Unauthorized("Você não pode deletar esse post!");
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return Ok("Post deletado com sucesso!");
    }
    [HttpGet("obter-post/{id}")]
    public async Task<IActionResult> BuscarPost(int id)
    {
        var post = await _context.Posts
        .Include(p => p.Autor)
        .Where(p => p.Id == id)
        .Select(p => new PostDTO
        {
            Id = p.Id,
            Titulo = p.Titulo,
            Conteudo = p.Conteudo,
            AutorNome = p.Autor.Nome,
            DataCriacao = p.DataCriacao
        }).FirstOrDefaultAsync();
        if (post == null) return NotFound("O post não foi encontrado!");

        return Ok(post);
    }
}