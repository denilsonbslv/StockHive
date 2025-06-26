using System.ComponentModel.DataAnnotations;

namespace StockHive.DTOs.Location;

/// <summary>
/// DTO para representar os dados de um local.
/// </summary>
public class LocationDto
{
    /// <summary>
    /// O ID do local.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// O nome do local.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// O endereço do local.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// A data e hora em que o local foi criado.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// A data e hora da última atualização do local, se houver.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}