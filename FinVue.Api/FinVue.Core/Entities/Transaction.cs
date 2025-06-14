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
    public string? CreationUserId { get; set; }
    public User PayingUser { get; set; }
    public string PayingUserId { get; set; }
    public Category? Category { get; set; }
    public string? CategoryId { get; set; }
    public string? RecurringTransactionId { get; set; }
    
    public Transaction(string id, string name, int valueInCent, DateOnly payDate, TransactionType type, PaymentMethod paymentMethod, User creationUser, User payingUser, Category category) {
        Id = id;
        Name = name;
        ValueInCent = valueInCent;
        PayDate = payDate;
        Type = type;
        PaymentMethod = paymentMethod;
        CreationUser = creationUser;
        CreationUserId = creationUser?.Id;
        PayingUser = payingUser;
        PayingUserId = payingUser.Id;
        Category = category;
        CategoryId = category?.Id;
        RecurringTransactionId = null;
    }

    public Transaction(string id, RecurringTransaction rt, User creationUser, DateOnly payDate) {
        Id = id;
        Name = rt.Name;
        ValueInCent = rt.ValueInCent;
        PayDate = payDate;
        Type = rt.Type;
        PaymentMethod = PaymentMethod.Überweisung;
        CreationUser = creationUser;
        CreationUserId = creationUser?.Id;
        PayingUser = creationUser!;
        PayingUserId = creationUser!.Id;
        Category = rt.Category;
        CategoryId = rt.Category?.Id;
        RecurringTransactionId = rt.Id;
    }

    public Transaction() {}
}
