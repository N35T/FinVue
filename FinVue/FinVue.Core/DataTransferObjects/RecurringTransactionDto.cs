using FinVue.Core.Entities;

namespace FinVue.Core.DataTransferObjects;
public class RecurringTransactionDto : RecurringTransaction
{

    public bool PayedThisMonth { get; set; }
    public RecurringTransactionDto(RecurringTransaction rt, bool payedThisMonth) : base(rt.Id, rt.Name, rt.ValueInCent, rt.MonthFrequency, rt.NextExecute, rt.Type, rt.Category)
    {
        PayedThisMonth = payedThisMonth;
    }
}
