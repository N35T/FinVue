using FinVue.Core.Enums;

namespace FinVue.Core.Entities {
    public class Transaction {
        string Id { get; set; }
        string Name { get; set; }
        int ValueInCent { get; set; }
        DateTime CreationDate { get; set; }
        DateOnly PayDate { get; set; }
        TransactionType Type { get; set; }
        PaymentMethod PaymentMethod { get; set; }
        User CreationUser { get; set; }
        User PayingUser { get; set; }
        Category Category { get; set; }
        RecurringTransaction? RecurringTransaction { get; set; }
        public Transaction(string id, string name, int valueInCent, DateTime creationDate, DateOnly payDate, TransactionType type, PaymentMethod paymentMethod, User creationUser, User payingUser, Category category, RecurringTransaction? recurringTransaction) {
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
            RecurringTransaction = recurringTransaction;
        }
        public Transaction(string id, string name, int valueInCent, DateTime creationDate, DateOnly payDate, TransactionType type, PaymentMethod paymentMethod, User creationUser, User payingUser, Category category) : this(id, name, valueInCent, creationDate, payDate, type, paymentMethod, creationUser, payingUser, category, null) { }
    }
}
