using FinVue.Core.Enums;

namespace FinVue.Core.Entities;
public class Transaction {
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
    
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateOnly PayDate { get; set; }
    public TransactionType Type { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public User? CreationUser { get; set; }
    public User PayingUser { get; set; }
    public Category? Category { get; set; }
    
    public Transaction(string id, string name, int valueInCent, DateOnly payDate, TransactionType type, PaymentMethod paymentMethod, User creationUser, User payingUser, Category category) {
        Id = id;
        Name = name;
        ValueInCent = valueInCent;
        PayDate = payDate;
        Type = type;
        PaymentMethod = paymentMethod;
        CreationUser = creationUser;
        PayingUser = payingUser;
        Category = category;
    }
    
    private Transaction() {}
}
