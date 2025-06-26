using System.ComponentModel.DataAnnotations;

namespace StockHive.DTOs.Supplier
{
    /// <summary>
    /// DTO para atualizar um fornecedor existente.
    /// </summary>
    public class UpdateSupplierDto
    {
        /// <summary>
        /// O novo nome do fornecedor (opcional).
        /// </summary>
        [MaxLength(255)]
        public string? Name { get; set; }

        /// <summary>
        /// A nova pessoa de contato no fornecedor (opcional).
        /// </summary>
        [MaxLength(150)]
        public string? ContactPerson { get; set; }

        /// <summary>
        /// O novo e-mail de contato do fornecedor (opcional).
        /// </summary>
        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// O novo telefone de contato do fornecedor (opcional).
        /// </summary>
        [MaxLength(20)]
        [Phone]
        public string? Phone { get; set; }

        /// <summary>
        /// O novo endereço do fornecedor (opcional).
        /// </summary>
        public string? Address { get; set; }
    }
}
