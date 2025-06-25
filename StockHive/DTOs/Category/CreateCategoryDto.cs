using System.ComponentModel.DataAnnotations;

namespace StockHive.DTOs.Category;

public class CreateCategoryDto
{
    [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
    [MaxLength(100)]
    public string Name { get; set; }

    public long? ParentCategoryId { get; set; }
}