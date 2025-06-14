﻿using FinVue.Core.DataTransferObjects;
using FinVue.Core.Entities;
using FinVue.Core.Enums;
using FinVue.Core.Exceptions;
using FinVue.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinVue.Core.Services;
public class TransactionService {
    private readonly IApplicationDbContext _dbContext;
        
    public TransactionService(IApplicationDbContext dbContext) {
            _dbContext = dbContext;
    }

    public async Task<Transaction?> GetTransactionFromIdAsync(string transactionId) {
        return await _dbContext.Transactions.FindAsync(transactionId);
    }

    public Task<List<Transaction>> GetAllTransactionsFromMonthAndYearAsync(int month, int year) {
        return _dbContext.Transactions
            .Where(e => e.PayDate.Month == month && e.PayDate.Year == year)
            .ToListAsync();
    }

    public Task<List<TransactionsByCategoryDto>> GetAllTransactionsFromMonthAndYearAndTypeGroupByCategoryAsync(int month, int year, TransactionType type) {
        return _dbContext.Transactions
            .Where(e => e.PayDate.Month == month && e.PayDate.Year == year && e.Type == type)
            .Include(e => e.PayingUser)
            .OrderBy(e => e.PayDate)
            .GroupBy(e => e.CategoryId)
            .Select(e => new TransactionsByCategoryDto { CategoryName = e.First().Category != null ? e.First().Category!.Name : "Misc", CategoryColor = e.First().Category != null ? e.First().Category!.CategoryColor.Hex : new Color(237, 227, 227, 144).Hex, TotalSum = e.Sum(t => t.ValueInCent), Transactions = e.ToList() })
            .ToListAsync();
    }

    public Task<List<Transaction>> GetAllTransactionsFromYearAndCategoryAsync(int year, string categoryId) {
        return _dbContext.Transactions
            .Where(e => e.CategoryId == categoryId && e.PayDate.Year == year)
            .ToListAsync();
    }

    public Task<List<Transaction>> GetAllTransactionsFromYearAsync(int year) {
        return _dbContext.Transactions
            .Where(e => e.PayDate.Year == year)
            .ToListAsync();
    }

    public Task<Transaction?> GetOldestTransactionAsync() {
        return _dbContext.Transactions
            .OrderBy(e => e.PayDate)
            .FirstOrDefaultAsync();
    }

    public virtual async Task<Transaction> AddTransactionAsync(Transaction transaction) {
        _dbContext.Transactions.Add(transaction);
        var changedRows = await _dbContext.SaveChangesAsync();

        if(changedRows > 0) { 
            return transaction;
        }

        throw new TransactionException("Failed adding the transaction: \n" + transaction.ToString());
    }

    public async Task<Transaction> DeleteTransactionAsync(string transactionId) {
        var transaction = await GetTransactionFromIdAsync(transactionId);

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

    public async Task<Transaction> ChangeCategoryFromTransactionAsync(string transactionId, string categoryId) {
        var transaction = await GetTransactionFromIdAsync(transactionId);
        var category = await _dbContext.Categories.FindAsync(categoryId);

        if (transaction is null) {
            throw new TransactionException("Failed changing the transaction: \n" + transactionId);
        }
        if(category is null) {
            throw new TransactionException("Couldn't find category with the id: \n" + categoryId);
        }

        transaction.Category = category;
        transaction.CategoryId = categoryId;

        var changedRows = await _dbContext.SaveChangesAsync();
        if (changedRows > 0) {
            return transaction;
        }
        throw new TransactionException("Failed changing the transaction: \n" + transaction.ToString());
    }

    public Task<int> GetTotalSumFromYearAsync(TransactionType type, int year) {
        return _dbContext.Transactions
            .Where(e => e.PayDate.Year == year && e.Type == type)
            .SumAsync(e => e.ValueInCent);
    }

    public async Task<List<int>> GetTotalSumFromYearAndAllRelevantMonthsAsync(TransactionType type, int year) {
        var sumByMonths = await _dbContext.Transactions
            .Where(e => e.PayDate.Year == year && e.Type == type)
            .GroupBy(e => e.PayDate.Month)
            .OrderBy(e => e.Key)
            .Select(e => new { Month = e.Key, Sum = e.Sum(t => t.ValueInCent) })
            .ToListAsync();

        if(sumByMonths.Count == 0) { 
            return new List<int>();
        }

        var lastMonth = sumByMonths.Max(x => x.Month);
        return Enumerable.Range(1, lastMonth)
            .Select(month => sumByMonths.FirstOrDefault(x => x.Month == month)?.Sum ?? 0)
            .ToList();
    }

    public Task<int> GetTotalSumFromYearAndMonthAsync(TransactionType type, int year, int month) {
        return _dbContext.Transactions
            .Where(e => e.PayDate.Year == year && e.PayDate.Month == month && e.Type == type)
            .SumAsync(e => e.ValueInCent);
    }

    public Task<int> GetTotalSumFromYearAndCategoryAsync(TransactionType type, int year, string categoryId) {
        return _dbContext.Transactions
            .Where(e => e.PayDate.Year == year && e.CategoryId == categoryId && e.Type == type)
            .SumAsync(e => e.ValueInCent);
    }

    public Task<int> GetTotalSumFromYearAndMonthAndCategoryAsync(TransactionType type, int year, int month, string categoryId) {
        return _dbContext.Transactions
            .Where(e => e.PayDate.Year == year && e.PayDate.Month == month && e.Type == type && e.CategoryId == categoryId)
            .SumAsync(e => e.ValueInCent);
    }
    
    public Task<List<SumByCategoryDto>> GetTotalSumsFromYearGroupedByCategoryAsync(int year, TransactionType type) {
        return _dbContext.Transactions
            .Where(e => e.PayDate.Year == year && e.Type == type)
            .Include(e => e.Category)
            .GroupBy(e => e.CategoryId)
            .Select(e => new SumByCategoryDto {
                CategoryName = e.First().Category != null ? e.First().Category!.Name : "Misc",
                CategoryColor = e.First().Category != null
                    ? e.First().Category!.CategoryColor.Hex
                    : new Color(237, 227, 227, 144).Hex,
                TotalSum = e.Sum(t => t.ValueInCent)
            })
            .ToListAsync();
    }
}
