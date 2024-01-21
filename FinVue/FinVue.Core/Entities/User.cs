namespace FinVue.Core.Entities
{
    public class User
    {
        string Id { get; set; }
        string Username { get; set; }
        public User(string id, string username) {
            Id = id;
            Username = username;
        }
    }
}