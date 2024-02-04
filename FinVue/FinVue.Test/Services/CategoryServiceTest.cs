using FinVue.Core.Entities;
using FinVue.Core.Interfaces;
using FinVue.Core.Services;

namespace FinVue.Test.Services; 

public class CategoryServiceTest {

    private CategoryService _sut;
    private TestDbContext _dbContext;

    public CategoryServiceTest() {
        _dbContext = new TestDbContext();
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        _dbContext.Categories.AddRange(GetTestCategories());
        _dbContext.SaveChanges();
        
        _sut = new CategoryService(_dbContext);
    }
    
    [Fact]
    public async Task CategoryService_Should_ReturnById() {
        var toFind = _dbContext.Categories.First();
        
        var found = await _sut.GetCategoryFromIdAsync(toFind.Id);
        
        Assert.Equal(toFind, found);
    }
    
    [Fact]
    public async Task CategoryService_ShouldNot_ReturnMissingId() {
        var found = await _sut.GetCategoryFromIdAsync("blablabla");
        
        Assert.Null(found);
    }
    
    [Fact]
    public async Task CategoryService_Should_ReturnAll() {
        var found = await _sut.GetAllCategoriesAsync();
        
        Assert.NotNull(found);
        Assert.Equal(7, found.Count);
    }
    
    [Fact]
    public async Task CategoryService_Should_AddCategory() {
        var category = new Category("blabla", "lel");
        
        var res = await _sut.AddCategoryAsync(category);
        
        Assert.NotNull(res);
        Assert.Equal(category, res);
    }
    
    [Fact]
    public async Task CategoryService_Should_ChangeName() {
        var toChange = _dbContext.Categories.First();
        
        var res = await _sut.ChangeCategoryNameFromIdAsync(toChange.Id, "blabla");
        
        Assert.NotNull(res);
        Assert.Equal(toChange, res);
        Assert.Equal("blabla", toChange.Name);
    }

    [Fact]
    public async Task CategoryService_Should_ChangeColor() {
        var toChange = _dbContext.Categories.First();
        var color = new Color(0, 0, 0, 255);
        
        var res = await _sut.ChangeCategoryColorFromIdAsync(toChange.Id, color);
        
        Assert.NotNull(res);
        Assert.Equal(toChange, res);
        Assert.Equal(color, toChange.CategoryColor);
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