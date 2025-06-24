using StockHive.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StockHive.Models
{
    /// <summary>
    /// Representa um fornecedor de produtos.
    /// </summary>
    public class Supplier : IAuditable
    {
        /// <summary>
        /// Identificador único do fornecedor.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Nome do fornecedor (obrigatório).
        /// </summary>
        [Required(ErrorMessage = "O nome do fornecedor é obrigatório.")]
        [MaxLength(255)]
        public required string Name { get; set; }

        /// <summary>
        /// Contato principal no fornecedor (opcional).
        /// </summary>
        [MaxLength(150)]
        public string? ContactPerson { get; set; }

        /// <summary>
        /// E-mail para contato (opcional).
        /// </summary>
        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string? Email { get; set; }

        /// <summary>
        /// Telefone de contato (opcional).
        /// </summary>
        [MaxLength(20)]
        [Phone(ErrorMessage = "Formato de telefone inválido.")]
        public string? Phone { get; set; }

        /// <summary>
        /// Endereço completo (opcional).
        /// </summary>
        public string? Address { get; set; }

        /// <inheritdoc/>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <inheritdoc/>
        public DateTime? UpdatedAt { get; set; }

        /// <inheritdoc/>
        public DateTime? DeletedAt { get; set; }
    }
}
