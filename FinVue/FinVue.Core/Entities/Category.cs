using System.ComponentModel.DataAnnotations.Schema;

namespace FinVue.Core.Entities;
public class Category {
    public string Id { get; set; }
    public string Name { get; set; }
    
    public Color CategoryColor { get; set; }
    
    public Category(string id, string name, Color color) {
        Id = id;
        Name = name;
        CategoryColor = color;
    }
}
