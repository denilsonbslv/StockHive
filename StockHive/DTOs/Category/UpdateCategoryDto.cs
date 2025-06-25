using System.ComponentModel.DataAnnotations;

namespace StockHive.DTOs.Category
{
    public class UpdateCategoryDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }
        public long? ParentCategoryId { get; set; }
    }
}
