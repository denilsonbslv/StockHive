namespace StockHive.QueryParameters;

/// <summary>
/// Define os parâmetros de consulta para filtrar e paginar categorias.
/// </summary>
public class CategoryQueryParameters
{
    /// <summary>
    /// Filtra as categorias pelo nome (busca parcial).
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Filtra as categorias por um ID de categoria pai específico.
    /// </summary>
    public long? ParentCategoryId { get; set; }

    /// <summary>
    /// Data de início para filtrar categorias criadas a partir desta data.
    /// </summary>
    public DateTime? CreatedAtFrom { get; set; } // Data de criação "a partir de"

    /// <summary>
    /// Data de fim para filtrar categorias criadas até esta data.
    /// </summary>
    public DateTime? CreatedAtTo { get; set; }   // Data de criação "até"

    // Parâmetros de Paginação
    private const int MaxPageSize = 100;
    private int _pageSize = 10; // Valor padrão

    /// <summary>
    /// Número da página atual para a paginação.
    /// </summary>
    public int PageNumber { get; set; } = 1; // Página padrão é a 1

    /// <summary>
    /// Tamanho da página (número de itens por página) para a paginação.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}