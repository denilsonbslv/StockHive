using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockHive.Models;

/// <summary>
/// Representa o inventário de um produto em um local específico.
/// </summary>
[Index(nameof(ProductId), nameof(LocationId), IsUnique = true, Name = "IX_Product_Location_Unique")]
public class Inventory
{
    /// <summary>
    /// Identificador único do registro de inventário.
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// Identificador do produto.
    /// </summary>
    [Required(ErrorMessage = "O ProductId do produto é obrigatório.")]
    public long ProductId { get; set; }

    /// <summary>
    /// Navegação para o produto.
    /// </summary>
    [ForeignKey("ProductId")]
    public required Product Product { get; set; }

    /// <summary>
    /// Identificador do local de armazenamento.
    /// </summary>
    [Required(ErrorMessage = "O LocationId do atributo é obrigatório.")]
    public long LocationId { get; set; }

    /// <summary>
    /// Navegação para o local de armazenamento.
    /// </summary>
    [ForeignKey("LocationId")]
    public required Location Location { get; set; }

    /// <summary>
    /// Quantidade atual do produto em estoque.
    /// </summary>
    public int Quantity { get; set; } = 0;

    /// <summary>
    /// Nível mínimo de estoque antes de um alerta ser gerado.
    /// </summary>
    public int LowStockThreshold { get; set; } = 10;

    /// <summary>
    /// Data da última atualização do registro de inventário.
    /// </summary>
    public DateTime LastUpdateAt { get; set; } = DateTime.UtcNow;
}