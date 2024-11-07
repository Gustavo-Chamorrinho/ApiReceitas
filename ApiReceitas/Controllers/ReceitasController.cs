using ApiReceitas.Context;
using ApiReceitas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiReceitas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceitasController : ControllerBase
    {
        private readonly ReceitasContext _context;

        public ReceitasController(ReceitasContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Receitas>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Receitas>>> GetReceitas()
        {
            return await _context.Receitas.ToListAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Receitas), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Receitas>> GetReceitasId(int id)
        {
            var receitas = await _context.Receitas.FindAsync(id);
            if (receitas is null) return null!;
            return receitas;
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(List<Receitas>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> SearchReceitas([FromQuery] string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                return BadRequest("O nome da receita deve ser fornecido.");
            }

            var receitas = await _context.Receitas
                .AsNoTracking()
                .Where(r => EF.Functions.ILike(r.Nome, $"%{nome}%"))
                .ToListAsync();

            if (receitas.Count == 0)
            {
                return NotFound("Nenhuma receita encontrada com o nome fornecido.");
            }

            return Ok(receitas);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Receitas), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Receitas>> CreateReceitas(Receitas receita)
        {
            _context.Receitas.Add(receita);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReceitas), new {id = receita.Id});
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Receitas), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Receitas>> Delete(int id)
        {
            var receita = await _context.Receitas.FindAsync(id);
            if (receita == null)
            {
                return NotFound();
            }
            _context.Receitas.Remove(receita);
            await _context.SaveChangesAsync();
            return Ok(receita);
        }

    }
}
