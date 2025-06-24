using StockHive.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockHive.Models;

/// <summary>
/// Categoria de produtos, suportando hierarquia via auto-relacionamento.
/// </summary>
public class Category : IAuditable
{
    /// <summary>
    /// Identificador único da categoria.
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// Nome da categoria.
    /// </summary>
    [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
    [MaxLength(100)]
    public required string Name { get; set; }

    /// <summary>
    /// Identificador opcional da categoria pai.
    /// </summary>
    public long? ParentCategoryId { get; set; }

    /// <summary>
    /// Referência para a categoria pai.
    /// </summary>
    [ForeignKey("ParentCategoryId")]
    public Category? ParentCategory { get; set; }

    /// <summary>
    /// Subcategorias associadas.
    /// </summary>
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();

    /// <inheritdoc/>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <inheritdoc/>
    public DateTime? UpdatedAt { get; set; }

    /// <inheritdoc/>
    public DateTime? DeletedAt { get; set; }
}