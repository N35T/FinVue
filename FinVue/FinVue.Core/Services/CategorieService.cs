
using FinVue.Core.Entities;
using FinVue.Core.Exceptions;
using FinVue.Core.Interfaces;

namespace FinVue.Core.Services;
internal class CategorieService {
    private readonly IApplicationDbContext _dbContext;

    public CategorieService(IApplicationDbContext dbContext) {
        _dbContext = dbContext;
    }
    public async Task<Category?> GetCategoryFromIdAsync(string categoryId) {
        return await _dbContext.Categories.FindAsync(categoryId);
    }
    public List<Category> GetAllCategoriesAsync() {
        return _dbContext.Categories.ToList();
    }

    public async Task<Category> AddCategoryAsync(Category category) {
        _dbContext.Categories.Add(category);
        var changedRows = await _dbContext.SaveChangesAsync();

        if (changedRows > 0) {
            return category;
        }

        throw new CategoryException("Failed adding the category: \n" + category.ToString());
    }

    public async Task<Category?> ChangeCategoryNameFromIdAsync(string categoryId, string newName) {
        var category = await GetCategoryFromIdAsync(categoryId);

        if (category is null) {
            throw new CategoryException("Failed changing the Category: \n" + categoryId);
        }

        category.Name = newName;

        var changedRows = await _dbContext.SaveChangesAsync();
        if (changedRows > 0) {
            return category;
        }
        throw new CategoryException("Failed changing the Category: \n" + category.ToString());
    }
    public async Task<Category?> ChangeCategoryColorFromIdAsync(string categoryId, Color newColor) {
        var category = await GetCategoryFromIdAsync(categoryId);

        if (category is null) {
            throw new CategoryException("Failed changing the Category: \n" + categoryId);
        }

        category.CategoryColor = newColor;

        var changedRows = await _dbContext.SaveChangesAsync();
        if (changedRows > 0) {
            return category;
        }
        throw new CategoryException("Failed changing the Category: \n" + category.ToString());
    }

    public async Task<Category?> DeleteCategoryFromIdAsync(string categoryId) {
        var category = await GetCategoryFromIdAsync(categoryId);

        if (category is null) {
            throw new CategoryException("Failed deleting the Category: \n" + categoryId);
        }

        _dbContext.Categories.Remove(category);

        var changedRows = await _dbContext.SaveChangesAsync();
        if (changedRows > 0) {
            return category;
        }
        throw new CategoryException("Failed deleting the Category: \n" + category.ToString());
    }
}

