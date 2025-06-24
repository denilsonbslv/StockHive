using StockHive.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockHive.Models;

public class Product : IAuditable
{
    [Key]
    public long Id { get; set; }

    [Required(ErrorMessage = "O SKU do produto é obrigatório.")]
    [MaxLength(100)]
    public string Sku { get; set; }

    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    [MaxLength(255)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public long CategoryId { get; set; }
    [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

    public long SupplierId { get; set; }
    [ForeignKey("SupplierId")]
        public Supplier? Supplier { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "O preço de venda deve ser maior que zero.")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal SalePrice { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}