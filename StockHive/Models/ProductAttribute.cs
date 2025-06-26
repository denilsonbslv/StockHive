using StockHive.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockHive.Models;

/// <summary>
/// Atributo de um produto, como cor ou tamanho.
/// </summary>
public class ProductAttribute : IAuditable
{
    /// <summary>
    /// Identificador único do atributo.
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// Identificador do produto ao qual o atributo pertence.
    /// </summary>
    [Required(ErrorMessage = "O ID do produto é obrigatório.")]
    public long ProductId { get; set; }

    /// <summary>
    /// Navegação para o produto.
    /// </summary>
    [ForeignKey("ProductId")]
    public Product? Product { get; set; }

    /// <summary>
    /// Nome do atributo (ex: "Cor").
    /// </summary>
    [Required(ErrorMessage = "O nome do atributo é obrigatório.")]
    [MaxLength(100)]
    public required string Name { get; set; }

    /// <summary>
    /// Valor do atributo (ex: "Azul").
    /// </summary>
    [Required(ErrorMessage = "O valor do atributo é obrigatório.")]
    [MaxLength(255)]
    public required string Value { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <inheritdoc/>
    public DateTime? UpdatedAt { get; set; }

    /// <inheritdoc/>
    public DateTime? DeletedAt { get; set; }
}