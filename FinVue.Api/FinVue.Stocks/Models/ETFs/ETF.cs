namespace FinVue.Stocks.Models;

public class ETF : Stock {
    
    public string FundManager { get; set; }
    public string IndexTracked { get; set; }
    public decimal ExpenseRatio { get; set; }

    public List<ETFRegionWeight> TopRegions { get; set; } = new();
    public List<ETFStockWeight> TopHoldings { get; set; } = new();

    public override string ToString() {
        return $"{base.ToString()} | ETF: Tracks {IndexTracked}, Expense Ratio: {ExpenseRatio:P}";
    }
}