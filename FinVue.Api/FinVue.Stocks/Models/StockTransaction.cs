namespace FinVue.Stocks.Models;

public class StockTransaction {
    public StockTransactionType Type { get; set; }
    public string Ticker { get; set; }
    public decimal Quantity { get; set; }
    public decimal PricePerShare { get; set; }
    public DateTime Timestamp { get; set; }
}