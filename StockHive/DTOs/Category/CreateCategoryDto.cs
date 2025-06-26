using System.ComponentModel.DataAnnotations;

namespace StockHive.DTOs.Category;

/// <summary>
/// DTO para criar uma nova categoria.
/// </summary>
public class CreateCategoryDto
{
    /// <summary>
    /// O nome da nova categoria.
    /// </summary>
    [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// O ID da categoria pai, se esta for uma subcategoria.
    /// </summary>
    public long? ParentCategoryId { get; set; }
}