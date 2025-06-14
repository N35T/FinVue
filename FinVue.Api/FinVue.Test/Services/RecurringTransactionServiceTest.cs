using FinVue.Core.Entities;
using FinVue.Core.Enums;
using FinVue.Core.Services;
using NSubstitute;

namespace FinVue.Test.Services; 

public class RecurringTransactionServiceTest {
    private RecurringTransactionService _sut;
    private TestDbContext _dbContext;

    public RecurringTransactionServiceTest() {
        _dbContext = new TestDbContext();
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        var cat = GetTestCategories();
        _dbContext.Categories.AddRange(cat);
        var rT = CreateTestRecurringTransactions(cat);
        _dbContext.RecurringTransactions.AddRange(rT);
        _dbContext.Transactions.AddRange(CreateTestTransactions(rT));
        _dbContext.SaveChanges();

        var transactionService = Substitute.For<TransactionService>(_dbContext);
        transactionService.AddTransactionAsync(Arg.Any<Transaction>()).Returns(t => (Transaction)t[0]);
        _sut = new RecurringTransactionService(_dbContext, transactionService);
    }

    [Fact]
    public async Task RecurringTransactionService_Should_AddRecurringTransaction() {
        var rt = new RecurringTransaction("afrf", "test", 100, 1, TransactionType.Einkommen, (string?)null);
        var res = await _sut.AddRecurringTransactionAsync(rt);
        
        Assert.NotNull(res);
        Assert.Equal(rt, res);
        Assert.True(_dbContext.RecurringTransactions.Any(e => e.Id == "afrf"));
    }

    [Fact]
    public async Task RecurringTransactionService_Should_ReturnById() {
        var toFind = _dbContext.RecurringTransactions.First();
        
        var found = await _sut.GetRecurringTransactionByIdAsync(toFind.Id);
        
        Assert.Equal(toFind, found);
    }
    
    [Fact]
    public async Task RecurringTransactionService_ShouldNot_ReturnMissingId() {
        var found = await _sut.GetRecurringTransactionByIdAsync("blablabla");
        
        Assert.Null(found);
    }
    
    [Fact]
    public async Task RecurringTransactionService_Should_ReturnAll() {
        var found = await _sut.GetAllRecurringTransactionsAsync();
        
        Assert.NotNull(found);
        Assert.Equal(2, found.Count);
    }
    
    [Fact]
    public async Task RecurringTransactionService_Should_ReturnAllFromMonth() {
        var res = await _sut.GetAllRecurringTransactionsFromMonthAsync(2024, Month.Januar);
        
        Assert.Equal(2, res.Count);
        Assert.True(res[1].PayedThisMonth);
        Assert.False(res[0].PayedThisMonth);
    }
    
    [Fact]
    public async Task RecurringTransactionService_Should_MarkRecurringTransactionAsDone() {
        var rt = _dbContext.RecurringTransactions.First();
        var user = _dbContext.Users.First();
        var monthFrequency = rt.MonthFrequency;
        
        var res = await _sut.MarkRecurringTransactionAsDone(rt.Id, new DateOnly(2024,1,1), user);
        
        Assert.NotNull(res);
        Assert.Equal(1, res.PayDate.Month);
        Assert.Equal(1, res.PayDate.Day);
        Assert.Equal(2024, res.PayDate.Year);
        Assert.Equal(rt.Name, res.Name);
    }
    
    [Fact]
    public async Task RecurringTransactionService_Should_DeleteTransaction() {
        var trans = _dbContext.RecurringTransactions.First();        
        
        var found = await _sut.DeleteRecurringTransactionFromIdAsync(trans.Id);
        
        Assert.NotNull(found);
        Assert.Equal(trans, found);
        Assert.Null(_dbContext.RecurringTransactions.Find(trans.Id));
    }
    
    private List<RecurringTransaction> CreateTestRecurringTransactions(List<Category> cat) {
        var i = 0;
        return new List<RecurringTransaction> {
            new RecurringTransaction(Guid.NewGuid().ToString(), "RecurringTransaction"+i++,10, 5, TransactionType.Einkommen, cat[0]),
            new RecurringTransaction(Guid.NewGuid().ToString(), "RecurringTransaction"+i++,100, 5, TransactionType.Einkommen, cat[0]),
        };
    }

    private List<Transaction> CreateTestTransactions(List<RecurringTransaction> rT) {
        var user = new User("blasdf", "TestUser");
        return new List<Transaction> {
            new Transaction(Guid.NewGuid().ToString(), rT[1], user, new DateOnly(2024,1,1))
        };
    }

    private List<Category> GetTestCategories() {
        return new List<Category> {
            new Category(Guid.NewGuid().ToString(), "Category1"),
            new Category(Guid.NewGuid().ToString(), "Category2"),
            new Category(Guid.NewGuid().ToString(), "Category3"),
            new Category(Guid.NewGuid().ToString(), "Category4"),
            new Category(Guid.NewGuid().ToString(), "Category5"),
            new Category(Guid.NewGuid().ToString(), "Category6"),
            new Category(Guid.NewGuid().ToString(), "Category7")
        };
    }
}