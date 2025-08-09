using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinVue.Stocks.DataModels;

public class DatabasePortfolio {
    
    [Key]
    public string Name { get; set; }
    
    [ForeignKey("PortfolioName")]
    public List<DatabasePortfolioTransaction> Transactions { get; set; } = new();

}