using FinVue.Core.DataTransferObjects;

namespace FinVue.Api.DataTransferObjects;

public class YearlyTransactionStatistics {

    public required int[] IncomePerMonth { get; set; }
    public required int[] OutcomePerMonth { get; set; }
    
    public required SumByCategoryDto[] OutcomeByCategory { get; set; }
}