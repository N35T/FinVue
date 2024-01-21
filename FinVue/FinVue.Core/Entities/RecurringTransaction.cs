using FinVue.Core.Enums;

namespace FinVue.Core.Entities
{
    public class RecurringTransaction
    {
        string Id { get; set; }
        string Name { get; set; }
        int ValueInCent { get; set; }
        int MonthFrequency { get; set; }
        TransactionType Type { get; set; }

        public RecurringTransaction(string id, string name, int valueInCent, int monthFrequency, TransactionType type) {
            Id = id;
            Name = name;
            ValueInCent = valueInCent;
            MonthFrequency = monthFrequency;
            Type = type;
        }
    }
}
