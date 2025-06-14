using FinVue.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinVue.Core.Interfaces;
public interface IApplicationDbContext {
    public DbSet<Category> Categories { get; }
    public DbSet<RecurringTransaction> RecurringTransactions { get; }
    public DbSet<Transaction> Transactions { get; }
    public DbSet<User> Users { get; }
    public Task<int> SaveChangesAsync();
}
