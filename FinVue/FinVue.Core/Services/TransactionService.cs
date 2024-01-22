using FinVue.Core.Entities;
using FinVue.Core.Exceptions;
using FinVue.Core.Interfaces;

namespace FinVue.Core.Services;
public class TransactionService {
    private readonly IApplicationDbContext _dbContext;
        
    public TransactionService(IApplicationDbContext dbContext) {
            _dbContext = dbContext;
    }

    public async Task<Transaction?> GetTransactionAsync(string transactionId) {
        return await _dbContext.Transactions.FindAsync(transactionId);
    }

    public async Task<Transaction?> GetAllTransactionsFromMonthAndYearAsync(int month, int year) {
        throw new NotImplementedException();
    }

    public async Task<Transaction> AddTransactionAsync(Transaction transaction) {
        _dbContext.Transactions.Add(transaction);
        var changedRows = await _dbContext.SaveChangesAsync();

        if(changedRows > 0) { 
            return transaction;
        }

        throw new TransactionException("Failed adding the transaction: \n" + transaction.ToString());
    }

    public async Task<Transaction> DeleteTransactionAsync(string transactionId) {
        var transaction = await GetTransactionAsync(transactionId);

        if (transaction is null) { 
            throw new TransactionException("Failed deleting the transaction: \n" + transactionId);
        }

        _dbContext.Transactions.Remove(transaction);

        var changedRows = await _dbContext.SaveChangesAsync();
        if (changedRows > 0) {
            return transaction;
        }
        throw new TransactionException("Failed deleting the transaction: \n" + transaction.ToString());
    }
}
