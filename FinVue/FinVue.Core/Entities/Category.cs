using System.ComponentModel.DataAnnotations.Schema;

namespace FinVue.Core.Entities;
public class Category {
    public string Id { get; set; }
    public string Name { get; set; }
    public Color CategoryColor { get; set; }

    public ICollection<Transaction> Transactions { get; private init; } = new List<Transaction>();

    public Category(string id, string name) : this(id, name, new Color(129, 169, 117, 255)) { }
    
    public Category(string id, string name, Color color) {
        Id = id;
        Name = name;
        CategoryColor = color;
    }
}
