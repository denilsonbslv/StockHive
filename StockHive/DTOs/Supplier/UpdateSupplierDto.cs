using System.ComponentModel.DataAnnotations;

namespace StockHive.DTOs.Supplier
{
    public class UpdateSupplierDto
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string? ContactPerson { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(20)]
        [Phone]
        public string? Phone { get; set; }

        public string? Address { get; set; }
    }
}
