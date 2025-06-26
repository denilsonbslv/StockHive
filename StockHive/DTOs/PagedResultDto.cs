namespace StockHive.DTOs;

/// <summary>
/// Representa um resultado paginado genérico para listas de DTOs.
/// </summary>
/// <typeparam name="T">O tipo do DTO contido na lista de itens.</typeparam>
public class PagedResultDto<T>
{
    /// <summary>
    /// O número da página atual.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// O tamanho da página (quantidade de itens por página).
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// O número total de registros que correspondem à consulta, antes da paginação.
    /// </summary>
    public int TotalRecords { get; set; }

    /// <summary>
    /// O número total de páginas, calculado com base em TotalRecords e PageSize.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

    /// <summary>
    /// A lista de itens (DTOs) para a página atual.
    /// </summary>
    public List<T> Items { get; set; } = new List<T>();
}