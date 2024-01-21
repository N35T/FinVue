using FinVue.Core.Enums;

namespace FinVue.Core.Entities;
public class RecurringTransaction {
    public string Id { get; set; }
    public string Name { get; set; }
    public int ValueInCent { get; set; }
    public int MonthFrequency { get; set; }
    public TransactionType Type { get; set; }
    
    public ICollection<Transaction> Transactions { get; set; }

    public RecurringTransaction(string id, string name, int valueInCent, int monthFrequency, TransactionType type) {
        Transactions = new List<Transaction>();
        Id = id;
        Name = name;
        ValueInCent = valueInCent;
        MonthFrequency = monthFrequency;
        Type = type;
    }
}