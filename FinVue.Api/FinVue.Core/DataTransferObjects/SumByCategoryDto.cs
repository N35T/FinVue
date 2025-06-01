using FinVue.Core.Entities;

namespace FinVue.Core.DataTransferObjects; 

public class SumByCategoryDto {
    public required string CategoryName { get; set; }
    public required int TotalSum { get; set; }
    
    public required Color CategoryColor { get; set; }
}