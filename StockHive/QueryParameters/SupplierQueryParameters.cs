namespace StockHive.QueryParameters;

/// <summary>
/// Define os parâmetros de consulta para filtrar e paginar fornecedores.
/// </summary>
public class SupplierQueryParameters
{
    /// <summary>
    /// Filtra os fornecedores pelo nome (busca parcial).
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Filtra os fornecedores pelo e-mail (busca parcial).
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Filtra os fornecedores pelo telefone (busca parcial).
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Data de início para filtrar fornecedores criados a partir desta data.
    /// </summary>
    public DateTime? CreatedAtFrom { get; set; }

    /// <summary>
    /// Data de fim para filtrar fornecedores criados até esta data.
    /// </summary>
    public DateTime? CreatedAtTo { get; set; }

    // Parâmetros de Paginação
    private const int MaxPageSize = 100;
    private int _pageSize = 10; // Valor padrão

    public int PageNumber { get; set; } = 1; // Página padrão é a 1

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}