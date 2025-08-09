using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinVue.Stocks.Models;

namespace FinVue.Stocks.DataModels;

public class DatabasePortfolioTransaction {
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public string PortfolioName { get; set; }
    
    [Required]
    public string Ticker { get; set; }
    [Required]
    public StockTransactionType TransactionType { get; set; }
    
    [Required]
    public decimal Quantity { get; set; }
    [Required]
    public decimal PricePerShare { get; set; }
    
    [NotMapped]
    private DateTime _timestamp = DateTime.Now;

    [Required]
    public DateTime Timestamp {
        get => DateTime.SpecifyKind(_timestamp, DateTimeKind.Utc);
        set => _timestamp = value;
    }
}