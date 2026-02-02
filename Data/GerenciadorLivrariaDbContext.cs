using GerenciadorLivraria.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorLivraria.Data
{
    public class GerenciadorLivrariaDbContext(DbContextOptions<GerenciadorLivrariaDbContext> options) : DbContext(options)
    {
        public DbSet<Livro> Livros { get; set; }
    }
}
