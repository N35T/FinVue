
using FinVue.Core.Entities;

namespace FinVue.Core.DataTransferObjects;
public class TransactionsByCategoryDto {
    public required string CategoryName { get; set; }
    public required List<Transaction> Transactions { get; set; }
    public required int TotalSum { get; set; }
    public required Color CategoryColor { get; set; }
}

