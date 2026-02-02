using GerenciadorLivraria.Enums;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorLivraria.Models
{
    public class Livro
    {
        /// <summary>
        /// Identificador único do livro gerado automaticamente pelo sistema.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Título do livro.
        /// Deve conter entre 2 e 120 caracteres.
        /// </summary>
        [Required(ErrorMessage = "O campo Title é obrigatório.")]
        [StringLength(120,MinimumLength=2,ErrorMessage= "O campo Title deve ter entre 2 e 120 caracteres.")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Autor do livro.
        /// Deve conter entre 2 e 120 caracteres.
        /// </summary>
        [Required(ErrorMessage ="O campo Author é obrigatório")]
        [StringLength(120,MinimumLength=2,ErrorMessage= "O campo Author deve ter entre 2 e 120 caracteres. ")]
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// Gênero do livro.
        /// Deve ser um valor válido do enum <see cref="GeneroLivro"/>.
        /// </summary>
        [Required]
        public GeneroLivro Genre { get; set; }

        /// <summary>
        /// Preço do livro.
        /// Deve ser maior ou igual a zero.
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        /// <summary>
        /// Quantidade disponível em estoque.
        /// Deve ser maior ou igual a zero.
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        /// <summary>
        /// Data de criação do registro.
        /// Preenchida automaticamente.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // <summary>
        /// Data da última atualização do registro.
        /// Atualizada somente em operações de alteração.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
