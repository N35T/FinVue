using FinVue.Core.Entities;

namespace FinVue.Core.DataTransferObjects;

public class CategoryDto {
    
    public required string Id { get; set; }
    public required string CategoryName { get; set; }
    public required string CategoryColor { get; set; }

    public static CategoryDto FromModel(Category category) {
        return new CategoryDto {
            Id = category.Id,
            CategoryName = category.Name,
            CategoryColor = category.CategoryColor.Hex,
        };
    }
}