using FinVue.Core.Enums;

namespace FinVue.Core.Entities;
public class RecurringTransaction {
    public string Id { get; set; }
    public string Name { get; set; }

    private int _valueInCent;
    public int ValueInCent {
        get => _valueInCent;
        set {
            if (value < 0) {
                throw new ArgumentException("Value can't be less than 0");
            }

            _valueInCent = value;
        }
    }

    private int _monthFrequency;
    public int MonthFrequency {
        get => _monthFrequency;
        set {
            if (value < 0) {
                throw new ArgumentException("Value can't be less than 0");
            }

            _monthFrequency = value;
        }
    }

    public TransactionType Type { get; set; }
    
    public Category Category { get; set; }
    
    public ICollection<Transaction> Transactions { get; private init; }

    public RecurringTransaction(string id, string name, int valueInCent, int monthFrequency, TransactionType type, Category category) {
        Transactions = new List<Transaction>();
        Id = id;
        Name = name;
        ValueInCent = valueInCent;
        MonthFrequency = monthFrequency;
        Type = type;
        Category = category;
    }
}