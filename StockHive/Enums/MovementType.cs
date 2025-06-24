namespace StockHive.Enums;

public enum MovementType
{
    PURCHASE = 1,       // Compra
    SALE = 2,           // Venda
    TRANSFER = 3,       // Transferência entre locais
    ADJUSTMENT_IN = 4,  // Ajuste de entrada (ex: achado no estoque)
    ADJUSTMENT_OUT = 5  // Ajuste de saída (ex: perda, dano)
}