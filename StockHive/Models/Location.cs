using StockHive.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StockHive.Models
{
    public class Location : IAuditable
    {
        // Corresponde a: id BIGINT PRIMARY KEY AUTO_INCREMENT
        [Key]
        public long Id { get; set; }

        // Corresponde a: name VARCHAR(255) NOT NULL
        [Required(ErrorMessage = "O nome do local é obrigatório.")]
        [MaxLength(255)]
        public string Name { get; set; }

        // Corresponde a: address VARCHAR(150)
        [MaxLength(150)]
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
