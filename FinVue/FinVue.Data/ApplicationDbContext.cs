using FinVue.Core.Entities;
using FinVue.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinVue.Data; 

public class ApplicationDbContext : DbContext, IApplicationDbContext{

    public DbSet<Category> Categories { get; private set; }
    public DbSet<RecurringTransaction> RecurringTransactions { get; private set; }
    public DbSet<Transaction> Transactions { get; private set; }
    public DbSet<User> Users { get; private set; }

    public ApplicationDbContext(DbContextOptions opt) : base(opt) {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        
        ConfigureCategory(modelBuilder.Entity<Category>());
        ConfigureRecurringTransaction(modelBuilder.Entity<RecurringTransaction>());
        ConfigureTransaction(modelBuilder.Entity<Transaction>());
        ConfigureUser(modelBuilder.Entity<User>());

        ConfigureRelations(modelBuilder);        
    }

    private static void ConfigureRelations(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Transaction>()
            .HasOne<RecurringTransaction>()
            .WithMany(e => e.Transactions)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Transaction>()
            .HasOne(e => e.Category)
            .WithMany(e => e.Transactions)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Transaction>()
            .HasOne(e => e.CreationUser)
            .WithMany()
            .HasForeignKey(e => e.CreationUserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Transaction>()
            .HasOne(e => e.PayingUser)
            .WithMany()
            .HasForeignKey(e => e.PayingUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecurringTransaction>()
            .HasOne(e => e.Category)
            .WithMany()
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }

    private static void ConfigureUser(EntityTypeBuilder<User> user) {
        user.HasKey(e => e.Id);
        user.Property(e => e.Username)
            .HasMaxLength(36);
    }
    
    private static void ConfigureTransaction(EntityTypeBuilder<Transaction> trans) {
        trans.HasKey(e => e.Id);
        trans
            .Property(e => e.Name)
            .HasMaxLength(50)
            .IsRequired();

        trans
            .Property(e => e.CreationDate)
            .IsRequired()
            .ValueGeneratedOnAdd();

        trans
            .Property(e => e.Type)
            .IsRequired();

        trans
            .Property(e => e.PayDate)
            .IsRequired();

        trans
            .Property(e => e.PaymentMethod)
            .IsRequired();

        trans
            .Property(e => e.ValueInCent)
            .IsRequired();
    }
    
    private static void ConfigureRecurringTransaction(EntityTypeBuilder<RecurringTransaction> rec) {
        rec.HasKey(e => e.Id);
        rec
            .Property(e => e.Name)
            .HasMaxLength(50)
            .IsRequired();

        rec
            .Property(e => e.MonthFrequency)
            .IsRequired();

        rec
            .Property(e => e.ValueInCent)
            .IsRequired();

        rec
            .Property(e => e.Type)
            .IsRequired();

    }

    private static void ConfigureCategory(EntityTypeBuilder<Category> cat) {
        cat.HasKey(e => e.Id);
        cat
            .Property(e => e.CategoryColor)
            .HasConversion(
                e => e.ToDto(),
                e => Color.FromDto(e)
            )
            .IsRequired();
        
        cat
            .Property(e => e.Name)
            .HasMaxLength(50)
            .IsRequired();
    }

    public Task<int> SaveChangesAsync() {
        return base.SaveChangesAsync();
    }
}