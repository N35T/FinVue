namespace FinVue.Core.Entities
{
    public class Category
    {
        string Id { get; set; }
        string Name { get; set; }

        public Category(string id, string name) {
            Id = id;
            Name = name;
        }
    }
}
