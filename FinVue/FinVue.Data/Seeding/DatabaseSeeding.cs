using FinVue.Core.Entities;
using FinVue.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinVue.Data.Seeding; 

public class DatabaseSeeding {

    private readonly ApplicationDbContext _dbContext;
    private Random _rng;

    public static User TestUsers { get; private set;}
    public static List<Category> TestCategories { get; private set;}
    public static List<Transaction> TestTransactions { get; private set;}
    public static List<RecurringTransaction> TestRecurringTransactions { get; private set; }
    
    internal DatabaseSeeding(ApplicationDbContext dbContext) {
        _dbContext = dbContext;
        _rng = new Random();
    }

    public async Task SeedAsync() {
        if (await _dbContext.Users.AnyAsync()) {
            TestUsers = await _dbContext.Users.FirstAsync();
            TestCategories = await _dbContext.Categories.Include(e => e.Transactions).ThenInclude(e => e.PayingUser).ToListAsync();
            TestTransactions = await _dbContext.Transactions.Include(e => e.Category).Include(e => e.PayingUser)
                .Include(e => e.CreationUser).ToListAsync();
            TestRecurringTransactions = await _dbContext.RecurringTransactions.Include(e => e.Category)
                .Include(e => e.Transactions).ThenInclude(e => e.PayingUser).ToListAsync(); 
            return;
        }
        
        await SeedUsersAsync();
        await SeedCategoriesAsync();
        await SeedTransactionsAsync();
        await SeedRecurringTransactionsAsync();
    }
    
    private Task SeedUsersAsync() {

        TestUsers = new User(Guid.NewGuid().ToString(), "TestUser");

        _dbContext.Users.Add(TestUsers);
        return _dbContext.SaveChangesAsync();
    }

    private Task SeedCategoriesAsync() {

        TestCategories = new List<Category> {
            new Category(Guid.NewGuid().ToString(), "Essen"),
            new Category(Guid.NewGuid().ToString(), "Miete"),
            new Category(Guid.NewGuid().ToString(), "Gehalt"),
            new Category(Guid.NewGuid().ToString(), "Versicherung"),
        };
        
        _dbContext.AddRange(TestCategories);
        return _dbContext.SaveChangesAsync();
    }

    private Task SeedTransactionsAsync() {

        TestTransactions = new List<Transaction>() {
            new Transaction(Guid.NewGuid().ToString(), "Kantine", 231, DateOnly.Parse("22.1.24"), TransactionType.Outcome, PaymentMethod.Card, TestUsers,TestUsers, TestCategories[0]),
            new Transaction(Guid.NewGuid().ToString(), "Kantine", 211, DateOnly.Parse("21.1.24"), TransactionType.Outcome, PaymentMethod.Card, TestUsers,TestUsers, TestCategories[0]),
            new Transaction(Guid.NewGuid().ToString(), "Kantine", 531, DateOnly.Parse("25.1.24"), TransactionType.Outcome, PaymentMethod.Card, TestUsers,TestUsers, TestCategories[0]),
            new Transaction(Guid.NewGuid().ToString(), "Kantine", 261, DateOnly.Parse("28.1.24"), TransactionType.Outcome, PaymentMethod.Card, TestUsers,TestUsers, TestCategories[0]),
            new Transaction(Guid.NewGuid().ToString(), "Kantine", 531, DateOnly.Parse("12.1.24"), TransactionType.Outcome, PaymentMethod.Card, TestUsers,TestUsers, TestCategories[0]),
            new Transaction(Guid.NewGuid().ToString(), "Kantine", 261, DateOnly.Parse("12.1.24"), TransactionType.Outcome, PaymentMethod.Card, TestUsers,TestUsers, TestCategories[0]),
            
            new Transaction(Guid.NewGuid().ToString(), "Miete", 44300, DateOnly.Parse("1.1.24"), TransactionType.Outcome, PaymentMethod.Transfer, TestUsers,TestUsers, TestCategories[1]),
            
            new Transaction(Guid.NewGuid().ToString(), "Gehalt", 500000, DateOnly.Parse("1.1.24"), TransactionType.Income, PaymentMethod.Card, TestUsers,TestUsers, TestCategories[2]),
        };

        TestCategories[0].Transactions.AddRange(TestTransactions[0..6]);
        TestCategories[1].Transactions.Add(TestTransactions[6]);
        TestCategories[2].Transactions.Add(TestTransactions[7]);
        
        _dbContext.Transactions.AddRange(TestTransactions);
        return _dbContext.SaveChangesAsync();
    }

    private Task SeedRecurringTransactionsAsync() {
        TestRecurringTransactions = new List<RecurringTransaction> {
            new RecurringTransaction(Guid.NewGuid().ToString(), "Miete", 44300, 1, Month.FEBUARY,
                TransactionType.Outcome, TestCategories[1]),
            new RecurringTransaction(Guid.NewGuid().ToString(), "Gehalt", 500000, 1, Month.FEBUARY,
                TransactionType.Income, TestCategories[2]),
            new RecurringTransaction(Guid.NewGuid().ToString(), "Autoversicherung", 200000, 1, Month.JANUARY,
                TransactionType.Outcome, TestCategories[3]),
        };
        
        TestRecurringTransactions[0].Transactions.Add(TestTransactions[6]);
        TestRecurringTransactions[1].Transactions.Add(TestTransactions[7]);

        _dbContext.RecurringTransactions.AddRange(TestRecurringTransactions);

        return _dbContext.SaveChangesAsync();
    }
    
}