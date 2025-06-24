using StockHive.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StockHive.Models
{
    public class Supplier : IAuditable
    {
        // Corresponde a: id BIGINT PRIMARY KEY AUTO_INCREMENT
        [Key]
        public long Id { get; set; }

        // Corresponde a: name VARCHAR(255) NOT NULL
        [Required(ErrorMessage = "O nome do fornecedor é obrigatório.")]
        [MaxLength(255)]
        public string Name { get; set; }

        // Corresponde a: contact_person VARCHAR(150)
        [MaxLength(150)]
        public string? ContactPerson { get; set; } // A '?' indica que o campo pode ser nulo (opcional)

        // Corresponde a: email VARCHAR(100)
        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string? Email { get; set; }

        // Corresponde a: phone VARCHAR(20)
        [MaxLength(20)]
        [Phone(ErrorMessage = "Formato de telefone inválido.")]
        public string? Phone { get; set; }

        // Corresponde a: address TEXT
        // Para campos TEXT/NVARCHAR(MAX), não precisamos de MaxLength.
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
