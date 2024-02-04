
using FinVue.Core.DataTransferObjects;
using FinVue.Core.Entities;
using FinVue.Core.Enums;
using FinVue.Core.Exceptions;
using FinVue.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinVue.Core.Services;
public class RecurringTransactionService {

    private readonly IApplicationDbContext _dbContext;
    private readonly TransactionService _tService;

    public RecurringTransactionService(IApplicationDbContext dbContext, TransactionService tService) {
        _dbContext = dbContext;
        _tService = tService;
    }
    // Add,
    public async Task<RecurringTransaction> AddRecurringTransactionAsync(RecurringTransaction rt) {
        _dbContext.RecurringTransactions.Add(rt);
        var changedRows = await _dbContext.SaveChangesAsync();

        if (changedRows > 0) {
            return rt;
        }

        throw new TransactionException("Failed adding the Recurring Transaction: \n" + rt.ToString());
    }
    // GetByID,
    public async Task<RecurringTransaction?> GetRecurringTransactionByIdAsync(string id) {
        return await _dbContext.RecurringTransactions.FindAsync(id);
    }
    // GetAll
    public Task<List<RecurringTransaction>> GetAllRecurringTransactionsAsync() {
        return _dbContext.RecurringTransactions.ToListAsync();
    }
    // Schauen ob es welche in diesem Monat gibt -> Liste an RT_DTO mit unbezahl und bezahlt 
    public Task<List<RecurringTransactionDto>> GetAllRecurringTransactionsFromMonthAsync(int year, Month month) {
        var unpayed = _dbContext.RecurringTransactions
            .Where(rt => rt.NextExecute == month)
            .Select(rt => new RecurringTransactionDto() {
                Category = rt.Category,
                CategoryId = rt.CategoryId,
                Id = rt.Id,
                NextExecute = rt.NextExecute,
                Name = rt.Name,
                MonthFrequency = rt.MonthFrequency,
                Type = rt.Type,
                ValueInCent = rt.MonthFrequency,
                PayedThisMonth = false
            });

        var payed = _dbContext.Transactions
            .Join(
                _dbContext.RecurringTransactions,
                t => t.RecurringTransactionId,
                rt => rt.Id,
                (t, rt) => new {RecurringTransaction = rt, Transaction = t }
            )
            .Where(t => t.Transaction.PayDate.Month == (int)month+1 && t.Transaction.PayDate.Year == year)
            .Select(t => new RecurringTransactionDto() {
                Category = t.RecurringTransaction.Category,
                CategoryId = t.RecurringTransaction.CategoryId,
                Id = t.RecurringTransaction.Id,
                NextExecute = t.RecurringTransaction.NextExecute,
                Name = t.RecurringTransaction.Name,
                MonthFrequency = t.RecurringTransaction.MonthFrequency,
                Type = t.RecurringTransaction.Type,
                ValueInCent = t.RecurringTransaction.MonthFrequency,
                PayedThisMonth = true
            });

        return unpayed
            .Concat(payed)
            .ToListAsync();
    }

    // Mark as done and Create Transaktion and save it
    public async Task<Transaction> MarkRecurringTransactionAsDone(string rtId, DateOnly payDate, User creationUser) {
        var rt = await GetRecurringTransactionByIdAsync(rtId);

        if (rt is null) {
            throw new TransactionException("Failed modifying the Recurring Transaction: \n" + rtId);
        }

        var id = Guid.NewGuid().ToString();
        var transaction = new Transaction(id, rt, creationUser, payDate);
        
        var nextExecute = (int)rt.NextExecute + rt.MonthFrequency;

        if (nextExecute > 12) {
            nextExecute -= 12;
        }

        rt.NextExecute = (Month)nextExecute;

        var changedRows = await _dbContext.SaveChangesAsync();
        if (changedRows <= 0) {
            throw new TransactionException("Failed modifying the Recurring Transaction: \n" + rt.ToString());
        }

        return await _tService.AddTransactionAsync(transaction);
    }

    // Delete,
    public async Task<RecurringTransaction?> DeleteRecurringTransactionFromIdAsync(string id) {
        var rt = await GetRecurringTransactionByIdAsync(id);

        if (rt is null) {
            throw new TransactionException("Failed deleting the Recurring Transaction: \n" + id);
        }

        _dbContext.RecurringTransactions.Remove(rt);

        var changedRows = await _dbContext.SaveChangesAsync();
        if (changedRows > 0) {
            return rt;
        }
        throw new TransactionException("Failed deleting the Recurring Transaction: \n" + rt.ToString());
    }
}

