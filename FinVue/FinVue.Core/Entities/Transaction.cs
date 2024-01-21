using FinVue.Core.Enums;

namespace FinVue.Core.Entities;
public class Transaction {
    public string Id { get; set; }
    public string Name { get; set; }
    public int ValueInCent { get; set; }
    public DateTime CreationDate { get; set; }
    public DateOnly PayDate { get; set; }
    public TransactionType Type { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public User CreationUser { get; set; }
    public User PayingUser { get; set; }
    public Category Category { get; set; }
    
    public Transaction(string id, string name, int valueInCent, DateTime creationDate, DateOnly payDate, TransactionType type, PaymentMethod paymentMethod, User creationUser, User payingUser, Category category) {
        Id = id;
        Name = name;
        ValueInCent = valueInCent;
        CreationDate = creationDate;
        PayDate = payDate;
        Type = type;
        PaymentMethod = paymentMethod;
        CreationUser = creationUser;
        PayingUser = payingUser;
        Category = category;
    }
}
