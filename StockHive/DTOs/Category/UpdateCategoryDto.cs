using System.ComponentModel.DataAnnotations;

namespace StockHive.DTOs.Category
{
    /// <summary>
    /// DTO para atualizar uma categoria existente.
    /// </summary>
    public class UpdateCategoryDto
    {
        /// <summary>
        /// O novo nome da categoria (opcional).
        /// </summary>
        [MaxLength(100)]
        public string? Name { get; set; }

        /// <summary>
        /// O novo ID da categoria pai (opcional).
        /// </summary>
        public long? ParentCategoryId { get; set; }
    }
}
