using System.ComponentModel.DataAnnotations;

namespace StockHive.DTOs.Location;

/// <summary>
/// DTO para criar uma nova Location.
/// </summary>
public class CreateLocationDto
{
    /// <summary>
    /// O nome do local.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A pessoa de contato no local (opcional).
    /// </summary>
    [MaxLength(150)]
    public string? Address { get; set; }
}