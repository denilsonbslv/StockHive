using System.ComponentModel.DataAnnotations;

namespace StockHive.DTOs.Category;

/// <summary>
/// DTO para representar os dados de uma categoria, incluindo suas subcategorias.
/// </summary>
public class CategoryDto
{
    /// <summary>
    /// O ID da categoria.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// O nome da categoria.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// O ID da categoria pai, se houver.
    /// </summary>
    public long? ParentCategoryId { get; set; }

    /// <summary>
    /// Uma coleção de DTOs que representam as subcategorias diretas.
    /// </summary>
    public ICollection<CategoryDto> SubCategories { get; set; } = new List<CategoryDto>();

    /// <summary>
    /// A data e hora em que a categoria foi criada.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// A data e hora da última atualização da categoria, se houver.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}