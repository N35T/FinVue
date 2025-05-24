using FinVue.Core.Interfaces;
using FinVue.Core.Services;
using FinVue.Data.Auth;
using FinVue.Data.Seeding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FinVue.Data; 

public static class DependencyInjection {

    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration) {

        services.AddDbContext<ApplicationDbContext>(opt => {
            opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddTransient<TransactionService>();
        services.AddTransient<RecurringTransactionService>();
        services.AddTransient<CategoryService>();
        services.AddTransient<UserService>();

        services.ConfigureAuth();

        return services;
    }

    private static void ConfigureAuth(this IServiceCollection services) {
        services.AddSingleton<JwtHandler>();
        services.AddAuthorization();
        
        services.AddAuthentication(opt => {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, null);

        // Add Jwt configuration after initialization
        services.AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtOptionsConfiguration>();
    }
    
    public static async Task EnsureDatabaseOnStartupAsync(this IServiceScope scope, bool isDevelopment) {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.EnsureCreatedAsync();

        if (isDevelopment) {
            var dbSeeding = new DatabaseSeeding(db);
            await dbSeeding.SeedAsync();
        }
    }
}