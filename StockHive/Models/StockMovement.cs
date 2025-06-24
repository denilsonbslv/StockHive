using StockHive.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockHive.Models;

/// <summary>
/// Representa um movimento de estoque, como compra, venda, transferência ou ajuste.
/// </summary>
public class StockMovement
{
    /// <summary>
    /// Identificador único do movimento de estoque.
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// Identificador do produto relacionado ao movimento.
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// Produto relacionado ao movimento de estoque.
    /// </summary>
    [ForeignKey("ProductId")]
    public Product Product { get; set; }

    /// <summary>
    /// Identificador do local de origem (opcional) do movimento.
    /// </summary>
    public long? FromLocationId { get; set; }

    /// <summary>
    /// Local de origem do movimento de estoque (pode ser nulo).
    /// </summary>
    [ForeignKey("FromLocationId")]
    public Location? FromLocation { get; set; }

    /// <summary>
    /// Identificador do local de destino (opcional) do movimento.
    /// </summary>
    public long? ToLocationId { get; set; }

    /// <summary>
    /// Local de destino do movimento de estoque (pode ser nulo).
    /// </summary>
    [ForeignKey("ToLocationId")]
    public Location? ToLocation { get; set; }

    /// <summary>
    /// Quantidade movimentada no estoque.
    /// </summary>
    [Required]
    public int QuantityMoved { get; set; }

    /// <summary>
    /// Tipo do movimento de estoque (compra, venda, transferência, ajuste, etc).
    /// </summary>
    [Required]
    public MovementType MovementType { get; set; } = MovementType.PURCHASE;

    /// <summary>
    /// Data e hora em que o movimento foi registrado.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Observações adicionais sobre o movimento de estoque.
    /// </summary>
    [MaxLength(1000)]
    public string? Notes { get; set; }
}