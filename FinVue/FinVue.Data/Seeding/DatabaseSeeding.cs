using FinVue.Core.Entities;
using FinVue.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinVue.Data.Seeding; 

internal class DatabaseSeeding {

    private readonly ApplicationDbContext _dbContext;
    private Random _rng;

    private User _user;
    private List<Category> _categories;
    private List<Transaction> _transactions;
    
    public DatabaseSeeding(ApplicationDbContext dbContext) {
        _dbContext = dbContext;
        _rng = new Random();
    }

    public async Task SeedAsync() {
        if (await _dbContext.Users.AnyAsync()) {
            return;
        }
        
        await SeedUsersAsync();
        await SeedCategoriesAsync();
        await SeedTransactionsAsync();
        await SeedRecurringTransactionsAsync();
    }
    
    private Task SeedUsersAsync() {

        _user = new User(Guid.NewGuid().ToString(), "TestUser");

        _dbContext.Users.Add(_user);
        return _dbContext.SaveChangesAsync();
    }

    private Task SeedCategoriesAsync() {

        _categories = new List<Category> {
            new Category(Guid.NewGuid().ToString(), "Essen"),
            new Category(Guid.NewGuid().ToString(), "Miete"),
            new Category(Guid.NewGuid().ToString(), "Gehalt"),
            new Category(Guid.NewGuid().ToString(), "Versicherung"),
        };
        
        _dbContext.AddRange(_categories);
        return _dbContext.SaveChangesAsync();
    }

    private Task SeedTransactionsAsync() {

        _transactions = new List<Transaction>() {
            new Transaction(Guid.NewGuid().ToString(), "Kantine", 231, DateOnly.Parse("22.1.24"), TransactionType.Outcome, PaymentMethod.Card, _user,_user, _categories[0]),
            new Transaction(Guid.NewGuid().ToString(), "Kantine", 211, DateOnly.Parse("21.1.24"), TransactionType.Outcome, PaymentMethod.Card, _user,_user, _categories[0]),
            new Transaction(Guid.NewGuid().ToString(), "Kantine", 531, DateOnly.Parse("25.1.24"), TransactionType.Outcome, PaymentMethod.Card, _user,_user, _categories[0]),
            new Transaction(Guid.NewGuid().ToString(), "Kantine", 261, DateOnly.Parse("28.1.24"), TransactionType.Outcome, PaymentMethod.Card, _user,_user, _categories[0]),
            new Transaction(Guid.NewGuid().ToString(), "Kantine", 531, DateOnly.Parse("12.1.24"), TransactionType.Outcome, PaymentMethod.Card, _user,_user, _categories[0]),
            new Transaction(Guid.NewGuid().ToString(), "Kantine", 261, DateOnly.Parse("12.1.24"), TransactionType.Outcome, PaymentMethod.Card, _user,_user, _categories[0]),
            
            new Transaction(Guid.NewGuid().ToString(), "Miete", 44300, DateOnly.Parse("1.1.24"), TransactionType.Outcome, PaymentMethod.Transfer, _user,_user, _categories[1]),
            
            new Transaction(Guid.NewGuid().ToString(), "Gehalt", 500000, DateOnly.Parse("1.1.24"), TransactionType.Income, PaymentMethod.Card, _user,_user, _categories[2]),
        };

        _dbContext.Transactions.AddRange(_transactions);
        return _dbContext.SaveChangesAsync();
    }

    private Task SeedRecurringTransactionsAsync() {
        var rec = new List<RecurringTransaction> {
            new RecurringTransaction(Guid.NewGuid().ToString(), "Miete", 44300, 1, Month.FEBUARY,
                TransactionType.Outcome, _categories[1]),
            new RecurringTransaction(Guid.NewGuid().ToString(), "Gehalt", 500000, 1, Month.FEBUARY,
                TransactionType.Income, _categories[2]),
            new RecurringTransaction(Guid.NewGuid().ToString(), "Autoversicherung", 200000, 1, Month.JANUARY,
                TransactionType.Outcome, _categories[3]),
        };
        
        rec[0].Transactions.Add(_transactions[6]);
        rec[1].Transactions.Add(_transactions[7]);

        _dbContext.RecurringTransactions.AddRange(rec);

        return _dbContext.SaveChangesAsync();
    }
    
}