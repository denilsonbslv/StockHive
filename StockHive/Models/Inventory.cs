using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockHive.Models;

[Index(nameof(ProductId), nameof(LocationId), IsUnique = true, Name = "IX_Product_Location_Unique")]
public class Inventory
{
    [Key]
    public long Id { get; set; }

    [Required(ErrorMessage = "O ProductId do produto é obrigatório.")]
    public long ProductId { get; set; }
    [ForeignKey("ProductId")]
        public Product Product { get; set; }

    [Required(ErrorMessage = "O LocationId do atributo é obrigatório.")]
    public long LocationId { get; set; }
    [ForeignKey("LocationId")]
        public Location Location { get; set; }

    public int Quantity { get; set; } = 0;

    public int LowStockThreshold { get; set; } = 10;

    public DateTime LastUpdateAt { get; set; } = DateTime.UtcNow;

}