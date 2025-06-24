using StockHive.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockHive.Models;

public class Category : IAuditable
{
    [Key]
    public long Id { get; set; }

    [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
    [MaxLength(100)]
    public string Name { get; set; }

    // --- AQUI ESTÁ A LÓGICA DO AUTO-RELACIONAMENTO ---

    // 1. A coluna da Chave Estrangeira (o "BIGINT NULL").
    // O EF Core usará esta propriedade para armazenar o ID da categoria pai.
    public long? ParentCategoryId { get; set; }

    // 2. A Propriedade de Navegação para o Pai.
    // Isso nos permite acessar o objeto da categoria pai diretamente no código.
    // Ex: var nomeDoPai = minhaCategoria.ParentCategory.Name;
    [ForeignKey("ParentCategoryId")]
    public Category? ParentCategory { get; set; }

    // 3. A Propriedade de Navegação para os Filhos (o outro lado da relação).
    // Isso nos permite obter uma lista de todas as subcategorias de uma categoria.
    // Ex: foreach(var sub in minhaCategoria.SubCategories) { ... }
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}