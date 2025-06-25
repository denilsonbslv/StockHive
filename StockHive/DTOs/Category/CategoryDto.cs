using System.ComponentModel.DataAnnotations;

namespace StockHive.DTOs.Category;

public class CategoryDto
{
    public long Id { get; set; }

    public string Name { get; set; }

    public long? ParentCategoryId { get; set; }

    public ICollection<CategoryDto> SubCategories { get; set; } = new List<CategoryDto>();

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}