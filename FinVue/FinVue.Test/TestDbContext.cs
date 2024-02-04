using FinVue.Core.Entities;
using FinVue.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinVue.Test; 

public class TestDbContext : DbContext, IApplicationDbContext {
    public DbSet<Category> Categories { get; set; }
    public DbSet<RecurringTransaction> RecurringTransactions { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set;  }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseInMemoryDatabase("TestDb");
    }

    public Task<int> SaveChangesAsync() {
        return base.SaveChangesAsync();
    }
}