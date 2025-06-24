using System.ComponentModel.DataAnnotations;

namespace StockHive.DTOs.Supplier;

public class SupplierDto
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string? ContactPerson { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [Phone]
    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}