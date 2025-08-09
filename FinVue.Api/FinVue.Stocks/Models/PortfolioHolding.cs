namespace FinVue.Stocks.Models;

public abstract class PortfolioHolding {
    
    public string Ticker { get; set; }
    public List<StockTransaction> Transactions { get; set; } = new();

    public decimal Quantity => Transactions
        .Where(t => t.Type is StockTransactionType.BUY or StockTransactionType.SELL)
        .Sum(t => t.Type == StockTransactionType.BUY ? t.Quantity : -t.Quantity);

    public decimal AverageCost {
        get {
            var buys = Transactions.Where(t => t.Type == StockTransactionType.BUY).ToList();
            var totalShares = buys.Sum(t => t.Quantity);
            var totalCost = buys.Sum(t => t.Quantity * t.PricePerShare);
            return totalShares > 0 ? totalCost / totalShares : 0;
        }
    }

    public abstract decimal GetMarketValue();

    public abstract HoldingType GetHoldingType();
    
}