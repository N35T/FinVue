using FinVue.Core.DataTransferObjects;

namespace FinVue.Api.DataTransferObjects;

public class MonthlyTransactions {
    
    public required List<TransactionsByCategoryDto> IncomeTransactions { get; set; }
    public required List<TransactionsByCategoryDto> OutcomeTransactions { get; set; }
}