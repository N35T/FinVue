using FinVue.Core.Entities;
using FinVue.Core.Enums;

namespace FinVue.Core.DataTransferObjects;
public class RecurringTransactionDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required int ValueInCent { get; set; }
    public required int MonthFrequency { get; set; }
    public required TransactionType Type { get; set; }
    public CategoryDto? Category { get; set; }
    public required bool PayedThisMonth { get; set; }

    public static RecurringTransactionDto FromModel(RecurringTransaction rt) {
        return new RecurringTransactionDto {
            Id = rt.Id,
            Name = rt.Name,
            ValueInCent = rt.ValueInCent,
            MonthFrequency = rt.MonthFrequency,
            Type = rt.Type,
            Category = rt.Category != null ? CategoryDto.FromModel(rt.Category) : null,
            PayedThisMonth = false
        };
    }

    public static RecurringTransactionDto FromModel(RecurringTransaction rt, bool payed) {
        var res = FromModel(rt);
        res.PayedThisMonth = payed;
        return res;
    }

}
