using FinVue.Core.Entities;
using FinVue.Core.Enums;

namespace FinVue.Core.DataTransferObjects;

public class TransactionDto {
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required int ValueInCent { get; set; }
    public required DateTime CreationDate { get; set; }
    public required DateOnly PayDate { get; set; }
    public required TransactionType Type { get; set; }
    public required PaymentMethod PaymentMethod { get; set; }
    public required User PayingUser { get; set; }
    public User? CreationUser { get; set; }
    public CategoryDto? Category { get; set; }
    public string? RecurringTransactionId { get; set; }

    public static TransactionDto FromModel(Transaction transaction) {
        return new TransactionDto() {
            Id = transaction.Id,
            Name = transaction.Name,
            ValueInCent = transaction.ValueInCent,
            CreationDate = transaction.CreationDate,
            CreationUser = transaction.CreationUser,
            PaymentMethod = transaction.PaymentMethod,
            PayingUser = transaction.PayingUser,
            Category = transaction.Category != null ? CategoryDto.FromModel(transaction.Category) : null,
            RecurringTransactionId = transaction.RecurringTransactionId,
            PayDate = transaction.PayDate,
            Type = transaction.Type
        };
    }
}