namespace FinVue.Stocks.Models;

public class Stock {
    
    public string Ticker { get; set; }
    public string Name { get; set; }
    public decimal CurrentPrice { get; set; }

    public decimal MarketCap { get; set; }
    public string Exchange { get; set; }
    public string Region { get; set; }
    public Currency Currency { get; set; }
    public DateTime LastUpdated { get; set; }

    public TimeScale HistoryScale { get; set; }
    public List<HistoricalPrice> History { get; set; } = new();

    public override string ToString() {
        return $"{Ticker} - {Name} @ {CurrentPrice:C}";
    }
    
}