using FinVue.Stocks.Models;

namespace FinVue.Stocks.Services;

public class PortfolioHistoryCalculator {
    
    public Dictionary<DateTime, decimal> CalculatePortfolioHistory(
        Portfolio portfolio,
        TimeScale timeScale,
        DateTime start,
        DateTime end) 
    {
        var history = new Dictionary<DateTime, decimal>();
        var timestamps = GenerateTimestamps(timeScale, start, end);

        foreach (var timestamp in timestamps) {
            decimal totalValue = 0;

            foreach (var holding in portfolio.Holdings) {
                var ownedQuantity = holding.Transactions
                    .Where(t => t.Timestamp <= timestamp)
                    .Sum(t => t.Type switch {
                        StockTransactionType.BUY => t.Quantity,
                        StockTransactionType.SELL => -t.Quantity,
                        _ => 0
                    });

                if (ownedQuantity <= 0) continue;
                
                var price = GetPriceAtTimestamp(holding, timestamp);
                totalValue += ownedQuantity * price;
            }
            history[timestamp] = totalValue;
        }
        return history;
    }

    private List<DateTime> GenerateTimestamps(TimeScale scale, DateTime start, DateTime end) {
        var result = new List<DateTime>();
        var current = start;

        while (current <= end) {
            result.Add(current);
            current = scale switch {
                TimeScale.Minute => current.AddMinutes(1),
                TimeScale.Hour => current.AddHours(1),
                TimeScale.Day => current.AddDays(1),
                TimeScale.Month => current.AddMonths(1),
                TimeScale.Year => current.AddYears(1),
                _ => current.AddDays(1)
            };
        }

        return result;
    }

    private decimal GetPriceAtTimestamp(PortfolioHolding holding, DateTime timestamp) {
        if (holding is StockPortfolioHolding stockHolding) {
            return FindClosestPrice(stockHolding.Stock.History, timestamp);
        }
        if (holding is ETFPortfolioHolding etfHolding) {
            return FindClosestPrice(etfHolding.ETF.History, timestamp);
        }

        return 0;
    }

    private decimal FindClosestPrice(List<HistoricalPrice> prices, DateTime timestamp) {
        var price = prices
            .OrderBy(p => Math.Abs((p.Timestamp - timestamp).TotalSeconds))
            .FirstOrDefault();

        return price?.Close ?? 0;
    }
}