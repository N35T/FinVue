namespace FinVue.Core.DataTransferObjects;

public class TransactionFromRecurringDto {
    public required string RecurringTransactionId { get; set; }
    public required DateOnly PayDate { get; set; }
}