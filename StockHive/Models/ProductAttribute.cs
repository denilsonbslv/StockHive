using StockHive.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockHive.Models;

public class ProductAttribute : IAuditable
{
    [Key]
    public long Id { get; set; }

    [Required(ErrorMessage = "O ID do produto é obrigatório.")]
    public long ProductId { get; set; }
    [ForeignKey("ProductId")]
        public Product? Product { get; set; }

    [Required(ErrorMessage = "O nome do atributo é obrigatório.")]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required(ErrorMessage = "O valor do atributo é obrigatório.")]
    [MaxLength(255)]
    public string Value { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}