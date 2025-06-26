using System.ComponentModel.DataAnnotations;

namespace StockHive.DTOs.Location
{
    /// <summary>
    /// DTO para atualizar um fornecedor existente.
    /// </summary>
    public class UpdateLocationDto
    {
        /// <summary>
        /// O novo nome do fornecedor (opcional).
        /// </summary>
        [MaxLength(255)]
        public string? Name { get; set; }

        /// <summary>
        /// O novo endereço do fornecedor (opcional).
        /// </summary> 
        [MaxLength(150)]
        public string? Address { get; set; }
    }
}
