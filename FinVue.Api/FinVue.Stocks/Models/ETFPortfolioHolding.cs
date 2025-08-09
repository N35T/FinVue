namespace FinVue.Stocks.Models;

public class ETFPortfolioHolding : PortfolioHolding {
    
    public ETF ETF { get; set; }
    
    public override decimal GetMarketValue() {
        return Quantity * ETF.CurrentPrice;
    }

    public override HoldingType GetHoldingType() {
        return HoldingType.ETF;
    }
}