using GerenciadorLivraria.Enums;

namespace GerenciadorLivraria.Dtos
{
    /// <summary>
    /// DTO utilizado para atualização parcial de um livro.
    /// Apenas os campos enviados serão alterados.
    /// </summary>
    public class UpdateLivroDto
    {
        /// <summary>
        /// Novo título do livro (opcional).
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Novo autor do livro (opcional).
        /// </summary>
        public string? Author { get; set; }

        /// <summary>
        /// Novo gênero do livro (opcional).
        /// </summary>
        public GeneroLivro? Genre { get; set; }

        /// <summary>
        /// Novo preço do livro (opcional).
        /// Deve ser maior ou igual a zero.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Novo estoque do livro (opcional).
        /// Deve ser maior ou igual a zero.
        /// </summary>
        public int? Stock { get; set; }
    }
}
