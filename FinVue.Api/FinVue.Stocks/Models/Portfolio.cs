namespace FinVue.Stocks.Models;

public class Portfolio {
    
    public string Name { get; set; } = "My Portfolio";
    public DateTime Created { get; set; } = DateTime.UtcNow;

    public List<PortfolioHolding> Holdings { get; set; } = new();

    public decimal TotalMarketValue => Holdings.Sum(h => h.GetMarketValue());

    public void AddHolding(PortfolioHolding holding) {
        Holdings.Add(holding);
    }

    public void RemoveHolding(string ticker) {
        Holdings.RemoveAll(h => h.Ticker.Equals(ticker, StringComparison.OrdinalIgnoreCase));
    }

    public PortfolioHolding? GetHolding(string ticker) {
        return Holdings.FirstOrDefault(h => h.Ticker.Equals(ticker, StringComparison.OrdinalIgnoreCase));
    }

    public override string ToString() {
        return $"{Name} | Total Value: {TotalMarketValue:C} | Holdings: {Holdings.Count}";
    }
}