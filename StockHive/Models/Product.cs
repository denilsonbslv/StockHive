using StockHive.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockHive.Models;

/// <summary>
/// Representa um produto comercializado, incluindo informações de preço, categoria e fornecedor.
/// </summary>
public class Product : IAuditable
{
    /// <summary>
    /// Identificador único do produto.
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// SKU único do produto (código de referência).
    /// </summary>
    [Required(ErrorMessage = "O SKU do produto é obrigatório.")]
    [MaxLength(100)]
    public required string Sku { get; set; }

    /// <summary>
    /// Nome do produto.
    /// </summary>
    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    [MaxLength(255)]
    public required string Name { get; set; }

    /// <summary>
    /// Descrição detalhada do produto (opcional).
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Identificador da categoria associada.
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// Navegação para a categoria do produto.
    /// </summary>
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

    /// <summary>
    /// Identificador do fornecedor associado.
    /// </summary>
    public long SupplierId { get; set; }

    /// <summary>
    /// Navegação para o fornecedor do produto.
    /// </summary>
    [ForeignKey("SupplierId")]
    public Supplier? Supplier { get; set; }

    /// <summary>
    /// Preço de custo do produto.
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }

    /// <summary>
    /// Preço de venda do produto.
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço de venda deve ser maior que zero.")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal SalePrice { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <inheritdoc/>
    public DateTime? UpdatedAt { get; set; }

    /// <inheritdoc/>
    public DateTime? DeletedAt { get; set; }
}