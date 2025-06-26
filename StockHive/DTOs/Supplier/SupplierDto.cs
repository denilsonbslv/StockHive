using System.ComponentModel.DataAnnotations;

namespace StockHive.DTOs.Supplier;

/// <summary>
/// DTO para representar os dados de um fornecedor.
/// </summary>
public class SupplierDto
{
    /// <summary>
    /// O ID do fornecedor.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// O nome do fornecedor.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A pessoa de contato no fornecedor (opcional).
    /// </summary>
    public string? ContactPerson { get; set; }

    /// <summary>
    /// O e-mail de contato do fornecedor (opcional).
    /// </summary>
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>
    /// O telefone de contato do fornecedor (opcional).
    /// </summary>
    [Phone]
    public string? Phone { get; set; }

    /// <summary>
    /// O endereço do fornecedor (opcional).
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// A data e hora em que o fornecedor foi criado.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// A data e hora da última atualização do fornecedor, se houver.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}