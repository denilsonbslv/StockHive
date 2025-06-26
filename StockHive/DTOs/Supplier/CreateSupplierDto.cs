using System.ComponentModel.DataAnnotations;

namespace StockHive.DTOs.Supplier;

/// <summary>
/// DTO para criar um novo fornecedor.
/// </summary>
public class CreateSupplierDto
{
    /// <summary>
    /// O nome do fornecedor.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A pessoa de contato no fornecedor (opcional).
    /// </summary>
    [MaxLength(150)]
    public string? ContactPerson { get; set; }

    /// <summary>
    /// O e-mail de contato do fornecedor (opcional).
    /// </summary>
    [MaxLength(100)]
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>
    /// O telefone de contato do fornecedor (opcional).
    /// </summary>
    [MaxLength(20)]
    [Phone]
    public string? Phone { get; set; }

    /// <summary>
    /// O endereço do fornecedor (opcional).
    /// </summary>
    public string? Address { get; set; }
}