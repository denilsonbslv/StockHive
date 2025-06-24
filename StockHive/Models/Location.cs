using StockHive.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StockHive.Models
{
    /// <summary>
    /// Representa um local de armazenamento físico.
    /// </summary>
    public class Location : IAuditable
    {
        /// <summary>
        /// Identificador único do local.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Nome do local (obrigatório).
        /// </summary>
        [Required(ErrorMessage = "O nome do local é obrigatório.")]
        [MaxLength(255)]
        public required string Name { get; set; }

        /// <summary>
        /// Endereço físico (opcional).
        /// </summary>
        [MaxLength(150)]
        public string? Address { get; set; }

        /// <inheritdoc/>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <inheritdoc/>
        public DateTime? UpdatedAt { get; set; }

        /// <inheritdoc/>
        public DateTime? DeletedAt { get; set; }
    }
}
