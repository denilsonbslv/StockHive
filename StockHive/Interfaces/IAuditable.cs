namespace StockHive.Interfaces;

/// <summary>
/// Define uma interface para entidades que precisam de controle de auditoria (data de criação, atualização e exclusão).
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// Data e hora da criação do registro.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data e hora da última atualização do registro.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Data e hora da exclusão lógica do registro.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}