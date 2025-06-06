
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
        return await _dbContext.RecurringTransactions
            .Include(e => e.Category)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }
    // GetAll
    public Task<List<RecurringTransaction>> GetAllRecurringTransactionsAsync() {
        return _dbContext.RecurringTransactions.ToListAsync();
    }
    // Schauen ob es welche in diesem Monat gibt -> Liste an RT_DTO mit unbezahl und bezahlt 
    public async Task<List<RecurringTransactionDto>> GetAllRecurringTransactionsFromMonthAsync(int year, Month month)
    {
        var unpayed = await _dbContext.RecurringTransactions
            .Include(r => r.Transactions)
            .Select(e => new
            {
                recurringTransaction = e,
                newestTransaction = e.Transactions.OrderByDescending(t => t.PayDate).FirstOrDefault(),
            })
            .Where(e => e.newestTransaction == null ||
                        (
                            (year - e.newestTransaction.PayDate.Year) * 12 +
                            (((int)month) - e.newestTransaction.PayDate.Month) > 0 &&
                            (year - e.newestTransaction.PayDate.Year) * 12 +
                            (((int)month) - e.newestTransaction.PayDate.Month) %
                            e.recurringTransaction.MonthFrequency == 0
                        )
            ).Select(e => e.recurringTransaction)
            .ToListAsync();

        var payed = await _dbContext.Transactions
            .Join(
                _dbContext.RecurringTransactions,
                t => t.RecurringTransactionId,
                rt => rt.Id,
                (t, rt) => new { RecurringTransaction = rt, Transaction = t }
            )
            .Where(t => t.Transaction.PayDate.Month == (int)month && t.Transaction.PayDate.Year == year)
            .Select(t => t.RecurringTransaction)
            .ToListAsync();

        return unpayed.Select(RecurringTransactionDto.FromModel)
            .Concat(payed.Select(e => RecurringTransactionDto.FromModel(e, true)))
            .ToList();
    }

    // Mark as done and Create Transaktion and save it
    public async Task<Transaction> MarkRecurringTransactionAsDone(string rtId, DateOnly payDate, User creationUser) {
        var rt = await GetRecurringTransactionByIdAsync(rtId);

        if (rt is null) {
            throw new TransactionException("Failed modifying the Recurring Transaction: \n" + rtId);
        }

        var id = Guid.NewGuid().ToString();
        var transaction = new Transaction(id, rt, creationUser, payDate);

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

