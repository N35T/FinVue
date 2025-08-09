namespace FinVue.Stocks.Models;

public class StockPortfolioHolding : PortfolioHolding {
    
    public Stock Stock { get; set; }

    public override decimal GetMarketValue() {
        return Quantity * Stock.CurrentPrice;
    }

    public override HoldingType GetHoldingType() {
        return HoldingType.STOCK;
    }
}