namespace StockHive.QueryParameters;

public class SupplierQueryParameters
{
    // Parâmetros de Filtro/Busca
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public DateTime? CreatedAtFrom { get; set; } // Data de criação "a partir de"
    public DateTime? CreatedAtTo { get; set; }   // Data de criação "até"

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