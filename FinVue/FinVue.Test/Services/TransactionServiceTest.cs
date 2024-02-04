using FinVue.Core.Entities;
using FinVue.Core.Enums;
using FinVue.Core.Services;

namespace FinVue.Test.Services; 

public class TransactionServiceTest : IDisposable, IAsyncDisposable {
    
    private TransactionService _sut;
    private TestDbContext _dbContext;

    public TransactionServiceTest() {
        _dbContext = new TestDbContext();
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        var cat = GetTestCategories();
        _dbContext.Categories.AddRange(cat);
        _dbContext.Transactions.AddRange(CreateTestTransactions(cat));
        _dbContext.SaveChanges();
        
        _sut = new TransactionService(_dbContext);
    }


    [Fact]
    public async Task TransactionService_Should_ReturnById() {
        var toFind = _dbContext.Transactions.First();
        
        var found = await _sut.GetTransactionFromIdAsync(toFind.Id);
        
        Assert.Equal(toFind, found);
    }
    
    [Fact]
    public async Task TransactionService_ShouldNot_ReturnMissingId() {
        var found = await _sut.GetTransactionFromIdAsync("blablabla");
        
        Assert.Null(found);
    }
    
    [Fact]
    public async Task TransactionService_Should_ReturnAllFromMonthYear() {
        var found = await _sut.GetAllTransactionsFromMonthAndYearAsync(1, 2024);
        
        Assert.NotNull(found);
        Assert.Equal(5, found.Count);
        Assert.All(found, t => {
            Assert.Equal(1, t.PayDate.Month);
            Assert.Equal(2024, t.PayDate.Year);
        });
    }
    
    [Fact]
    public async Task TransactionService_Should_ReturnAllFromYearAndCategory() {
        var category = _dbContext.Categories.First();
        var found = await _sut.GetAllTransactionsFromYearAndCategoryAsync(2024, category.Id);
        
        Assert.NotNull(found);
        Assert.All(found, t => {
            Assert.Equal(category.Id, t.CategoryId);
            Assert.Equal(2024, t.PayDate.Year);
        });
    }

    [Fact]
    public async Task TransactionService_Should_ReturnAllYear() {
        var category = _dbContext.Categories.First();
        var found = await _sut.GetAllTransactionsFromYearAsync(2024);
        
        Assert.NotNull(found);
        Assert.Equal(11, found.Count);
        Assert.All(found, t => {
            Assert.Equal(2024, t.PayDate.Year);
        });
    }
    
    [Fact]
    public async Task TransactionService_Should_AddTransaction() {
        var category = _dbContext.Categories.First();
        var user = _dbContext.Users.First();
        var trans = new Transaction("balbal", "Test", 10000, DateOnly.Parse("1.1.1999"), TransactionType.Outcome,
            PaymentMethod.Card, user, user, category);
        
        var found = await _sut.AddTransactionAsync(trans);
        
        Assert.NotNull(found);
        Assert.Equal(trans, found);
        Assert.Equal(found, _dbContext.Transactions.Find("balbal"));
    }
    
    [Fact]
    public async Task TransactionService_Should_DeleteTransaction() {
        var trans = _dbContext.Transactions.First();        
        
        var found = await _sut.DeleteTransactionAsync(trans.Id);
        
        Assert.NotNull(found);
        Assert.Equal(trans, found);
        Assert.Null(_dbContext.Transactions.Find(trans.Id)); 
    }
    
    [Fact]
    public async Task TransactionService_Should_ChangeCategoryOfTransaction() {
        var trans = _dbContext.Transactions.First();
        var newcategory = _dbContext.Categories.First(e => e.Id != trans.CategoryId); 
        
        var found = await _sut.ChangeCategoryFromTransactionAsync(trans.Id, newcategory.Id);
        
        Assert.NotNull(found);
        Assert.Equal(trans, found);
        Assert.Equal(newcategory.Id, trans.CategoryId);
    }
    
    [Fact]
    public async Task TransactionService_Should_GetTotalOutcomeSumFromYear() {
        var res = await _sut.GetTotalSumFromYearAsync(TransactionType.Outcome, 2024);
        
        Assert.Equal(2605, res);
    }
    
    [Fact]
    public async Task TransactionService_Should_GetTotalIncomeSumFromYear() {
        var res = await _sut.GetTotalSumFromYearAsync(TransactionType.Income, 2024);
        
        Assert.Equal(598, res);
    }
    
    [Fact]
    public async Task TransactionService_Should_GetTotalOutcomeSumFromYearByMonth() {
        var res = await _sut.GetTotalSumFromYearAndAllMonthsAsync(TransactionType.Outcome, 2024);
        
        Assert.Equal(105, res[0]);
        Assert.Equal(1697, res[1]);
        Assert.Equal(803, res[2]);
    }
    
    [Fact]
    public async Task TransactionService_Should_GetTotalIncomeSumFromYearByMonth() {
        var res = await _sut.GetTotalSumFromYearAndAllMonthsAsync(TransactionType.Income, 2024);
        
        Assert.Equal(50, res[0]);
        Assert.Equal(340, res[1]);
        Assert.Equal(208, res[2]);
    }
    
    [Fact]
    public async Task TransactionService_Should_GetTotalOutcomeSumFromYearAndMonth() {
        var res = await _sut.GetTotalSumFromYearAndMonthAsync(TransactionType.Outcome, 2024, 1);
        
        Assert.Equal(105, res);
    }
    
    [Fact]
    public async Task TransactionService_Should_GetTotalIncomeSumFromYearAndMonth() {
        var res = await _sut.GetTotalSumFromYearAndMonthAsync(TransactionType.Income, 2024, 1);
        
        Assert.Equal(50, res);
    }
    
    [Fact]
    public async Task TransactionService_Should_GetTotalOutcomeSumFromYearAndCategory() {
        var category = _dbContext.Categories.First(e => e.Name == "Category1");
        var res = await _sut.GetTotalSumFromYearAndCategoryAsync(TransactionType.Outcome, 2024, category.Id);
        
        Assert.Equal(0, res);
    }
    
    [Fact]
    public async Task TransactionService_Should_GetTotalIncomeSumFromYearAndCategory() {
        var category = _dbContext.Categories.First(e => e.Name == "Category1");
        var res = await _sut.GetTotalSumFromYearAndCategoryAsync(TransactionType.Income, 2024, category.Id);
        
        Assert.Equal(350, res);
    }
    
    [Fact]
    public async Task TransactionService_Should_GetTotalOutcomeSumFromYearAndMonthAndCategory() {
        var category = _dbContext.Categories.First(e => e.Name == "Category2");
        var res = await _sut.GetTotalSumFromYearAndMonthAndCategoryAsync(TransactionType.Outcome, 2024, 2,category.Id);
        
        Assert.Equal(610, res);
    }
    
    [Fact]
    public async Task TransactionService_Should_GetTotalIncomeSumFromYearAndMonthAndCategory() {
        var category = _dbContext.Categories.First(e => e.Name == "Category2");
        var res = await _sut.GetTotalSumFromYearAndMonthAndCategoryAsync(TransactionType.Income, 2024,  1,category.Id);
        
        Assert.Equal(20, res);
    }
    
    [Fact]
    public async Task TransactionService_Should_GetTotalOutcomeSumsFromYearGroupedByCategory() {
        var user = _dbContext.Users.First();
        var newtrans = new Transaction(Guid.NewGuid().ToString(), "Transaction5623", 5023, DateOnly.Parse("6.9.24"),
            TransactionType.Outcome, PaymentMethod.Card, user, user, null);
        _dbContext.Transactions.Add(newtrans);
        await _dbContext.SaveChangesAsync();
        
        
        var res = await _sut.GetTotalSumsFromYearGroupedByCategoryAsync(2024, TransactionType.Outcome);
        
        Assert.Equal(5, res.Count);
        var cat2 = res.Single(e => e.CategoryName == "Category2");
        Assert.NotNull(cat2);
        Assert.Equal(610, cat2.TotalSum);
        var cat3 = res.Single(e => e.CategoryName == "Category3");
        Assert.NotNull(cat3);
        Assert.Equal(1092, cat3.TotalSum); 
        var cat4 = res.Single(e => e.CategoryName == "Category4");
        Assert.NotNull(cat4);
        Assert.Equal(100, cat4.TotalSum);
        var cat6 = res.Single(e => e.CategoryName == "Category6");
        Assert.NotNull(cat6);
        Assert.Equal(803, cat6.TotalSum);
        var misc = res.Single(e => e.CategoryName == "Misc");
        Assert.NotNull(misc);
        Assert.Equal(5023, misc.TotalSum);
    }
    
    [Fact]
    public async Task TransactionService_Should_GetTotalIncomeSumsFromYearGroupedByCategory() {
        var user = _dbContext.Users.First();
        var newtrans = new Transaction(Guid.NewGuid().ToString(), "Transaction5623", 5023, DateOnly.Parse("6.9.24"),
            TransactionType.Income, PaymentMethod.Card, user, user, null);
        _dbContext.Transactions.Add(newtrans);
        await _dbContext.SaveChangesAsync();
        
        
        var res = await _sut.GetTotalSumsFromYearGroupedByCategoryAsync(2024, TransactionType.Income);
        
        Assert.Equal(5, res.Count);
        var cat1 = res.Single(e => e.CategoryName == "Category1");
        Assert.NotNull(cat1);
        Assert.Equal(350, cat1.TotalSum); 
        var cat2 = res.Single(e => e.CategoryName == "Category2");
        Assert.NotNull(cat2);
        Assert.Equal(20, cat2.TotalSum);
        var cat4 = res.Single(e => e.CategoryName == "Category4");
        Assert.NotNull(cat4);
        Assert.Equal(102, cat4.TotalSum);
        var cat5 = res.Single(e => e.CategoryName == "Category5");
        Assert.NotNull(cat5);
        Assert.Equal(126, cat5.TotalSum); 
        var misc = res.Single(e => e.CategoryName == "Misc");
        Assert.NotNull(misc);
        Assert.Equal(5023, misc.TotalSum);
    }
    
    private List<Transaction> CreateTestTransactions(List<Category> categories) {
        var testUser = new User("blabla" ,"testuser");
        var i = 1;
        var transactions = new List<Transaction> {
            new Transaction(Guid.NewGuid().ToString(), "Transaction" + i++, 10, DateOnly.Parse("4.1.23"), TransactionType.Income, PaymentMethod.Card, testUser,testUser, categories[0]),
            new Transaction(Guid.NewGuid().ToString(), "Transaction" + i++, 10, DateOnly.Parse("4.1.24"), TransactionType.Income, PaymentMethod.Card, testUser,testUser, categories[0]),
            new Transaction(Guid.NewGuid().ToString(), "Transaction" + i++, 20, DateOnly.Parse("6.1.24"), TransactionType.Income, PaymentMethod.Card, testUser,testUser, categories[1]),
            new Transaction(Guid.NewGuid().ToString(), "Transaction" + i++, 5, DateOnly.Parse("7.1.24"), TransactionType.Outcome, PaymentMethod.Card, testUser,testUser, categories[2]),
            new Transaction(Guid.NewGuid().ToString(), "Transaction" + i++, 100, DateOnly.Parse("7.1.24"), TransactionType.Outcome, PaymentMethod.Card, testUser,testUser, categories[3]),
            new Transaction(Guid.NewGuid().ToString(), "Transaction" + i++, 20, DateOnly.Parse("10.1.24"), TransactionType.Income, PaymentMethod.Card, testUser,testUser, categories[4]),
            new Transaction(Guid.NewGuid().ToString(), "Transaction" + i++, 340, DateOnly.Parse("3.2.24"), TransactionType.Income, PaymentMethod.Card, testUser,testUser, categories[0]),
            new Transaction(Guid.NewGuid().ToString(), "Transaction" + i++, 610, DateOnly.Parse("15.2.24"), TransactionType.Outcome, PaymentMethod.Card, testUser,testUser, categories[1]),
            new Transaction(Guid.NewGuid().ToString(), "Transaction" + i++, 1087, DateOnly.Parse("23.2.24"), TransactionType.Outcome, PaymentMethod.Card, testUser,testUser, categories[2]),
            new Transaction(Guid.NewGuid().ToString(), "Transaction" + i++, 102, DateOnly.Parse("1.3.24"), TransactionType.Income, PaymentMethod.Card, testUser,testUser, categories[3]),
            new Transaction(Guid.NewGuid().ToString(), "Transaction" + i++, 106, DateOnly.Parse("3.3.24"), TransactionType.Income, PaymentMethod.Card, testUser,testUser, categories[4]),
            new Transaction(Guid.NewGuid().ToString(), "Transaction" + i++, 803, DateOnly.Parse("6.3.24"), TransactionType.Outcome, PaymentMethod.Card, testUser,testUser, categories[5]),
        };

        return transactions;
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


    public async ValueTask DisposeAsync() {
        await _dbContext.DisposeAsync();
    }

    public void Dispose() {
        _dbContext.Dispose();
    }
}