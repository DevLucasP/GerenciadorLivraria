using GerenciadorLivraria.Data;
using GerenciadorLivraria.Dtos;
using GerenciadorLivraria.Enums;
using GerenciadorLivraria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorLivraria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly GerenciadorLivrariaDbContext _context;

        public BooksController(GerenciadorLivrariaDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todos os livros cadastrados na livraria.
        /// </summary>
        /// <returns>
        /// Retorna uma lista de livros ou nenhum conteúdo se não houver registros.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var livros = await _context.Livros.ToListAsync();
            if(livros.Count == 0)
                return NoContent();

            return Ok(livros);
        }

        /// <summary>
        /// Busca livros pelo título ou autor com paginação.
        /// </summary>
        /// <param name="valor">Texto usado para busca no título ou autor.</param>
        /// <param name="page">Número da página (padrão: 1).</param>
        /// <param name="pageSize">Quantidade de itens por página (padrão: 10).</param>
        /// <returns>
        /// Lista paginada contendo título, gênero, preço e estoque.
        /// </returns>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookByName(
            [FromQuery] string valor,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return BadRequest("Informe um valor para busca.");

            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page e PageSize devem ser maiores que zero.");

            var query = _context.Livros
                .Where(l =>
                    l.Title.Contains(valor) ||
                    l.Author.Contains(valor));

            var totalItens = await query.CountAsync();

            var livros = await query
                .OrderBy(l => l.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(l => new
                {
                    l.Title,
                    l.Genre,
                    l.Price,
                    l.Stock
                })
                .ToListAsync();

            if (!livros.Any())
                return NotFound("Nenhum livro encontrado.");

            return Ok(new
            {
                page,
                pageSize,
                totalItens,
                totalPaginas = (int)Math.Ceiling(totalItens / (double)pageSize),
                livros
            });
        }

        /// <summary>
        /// Busca um livro pelo ID.
        /// </summary>
        /// <param name="id">
        /// Identificador único (GUID) do livro.
        /// </param>
        /// <returns>
        /// Retorna os dados do livro caso encontrado.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var livro = await _context.Livros.FindAsync(id);
            if (livro == null)
                return NotFound("Livro não encontrado");
            return Ok(livro); 
        }

        /// <summary>
        /// Cria um novo livro na livraria.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /api/books
        ///     {
        ///        "title": "O Senhor dos Anéis",
        ///        "author": "J. R. R. Tolkien",
        ///        "genre": "Fantasia",
        ///        "price": 59.90,
        ///        "stock": 10
        ///     }
        ///
        /// </remarks>
        /// <returns>
        /// Retorna o livro criado.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PostBook([FromBody] Livro livro)
        {
           if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var livroExiste = await _context.Livros
                .AnyAsync(l => l.Title == livro.Title && l.Author == livro.Author);

            if (livroExiste)
                return Conflict("Erro: Livro já cadastrado");


            await _context.Livros.AddAsync(livro);
            await _context.SaveChangesAsync();
            return StatusCode(201, livro);
        }

        /// <summary>
        /// Atualiza parcialmente os dados de um livro.
        /// </summary>
        /// <param name="id">
        /// Identificador único (GUID) do livro.
        /// </param>
        /// <remarks>
        /// É possível atualizar apenas os campos desejados.
        ///
        /// Exemplo 1 - Atualizar somente o preço:
        ///
        ///     PUT /api/books/{id}
        ///     {
        ///         "price": 40.00
        ///     }
        ///
        /// Exemplo 2 - Atualizar título e autor:
        ///
        ///     PUT /api/books/{id}
        ///     {
        ///         "title": "Clean Code",
        ///         "author": "Robert C. Martin"
        ///     }
        ///
        /// </remarks>
        /// <returns>
        /// Retorna 204 se a atualização for realizada com sucesso.
        /// </returns>

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<IActionResult> PutBook(Guid id, [FromBody] UpdateLivroDto dto)
        {
            // Buscar livro existente
            var livro = await _context.Livros.FindAsync(id);

            if (livro == null)
                return NotFound("Livro não encontrado.");

            // Definir estado FINAL (para duplicidade)
            var novoTitle = !string.IsNullOrWhiteSpace(dto.Title)
                ? dto.Title.Trim()
                : livro.Title;

            var novoAuthor = !string.IsNullOrWhiteSpace(dto.Author)
                ? dto.Author.Trim()
                : livro.Author;

            // Validar duplicidade
            bool existeDuplicado = await _context.Livros.AnyAsync(l =>
                l.Id != id &&
                l.Title == novoTitle &&
                l.Author == novoAuthor
            );

            if (existeDuplicado)
                return Conflict("Já existe um livro com o mesmo título e autor.");

            // Atualizar Title e Author
            livro.Title = novoTitle;
            livro.Author = novoAuthor;

            // Atualizar gênero
            if (dto.Genre.HasValue &&
                Enum.IsDefined(typeof(GeneroLivro), dto.Genre.Value))
            {
                livro.Genre = dto.Genre.Value;
            }

            // Atualizar preço
            if (dto.Price.HasValue)
            {
                if (dto.Price < 0)
                    return BadRequest("Erro: Preço não pode ser negativo.");

                livro.Price = dto.Price.Value;
            }

            // Atualizar estoque
            if (dto.Stock.HasValue)
            {
                if (dto.Stock < 0)
                    return BadRequest("Erro: Estoque não pode ser negativo.");

                livro.Stock = dto.Stock.Value;
            }

            // Atualizar data
            livro.UpdatedAt = DateTime.UtcNow;

            // Salvar
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Remove um livro da livraria.
        /// </summary>
        /// <param name="id">
        /// Identificador único (GUID) do livro.
        /// </param>
        /// <returns>
        /// Retorna 204 se o livro for removido com sucesso.
        /// </returns>

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            // Buscar livro pelo ID
            var livro = await _context.Livros.FindAsync(id);

            if (livro == null)
                return NotFound("Livro não encontrado.");

            // Remover livro
            _context.Livros.Remove(livro);

            // Salvar alteração
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
