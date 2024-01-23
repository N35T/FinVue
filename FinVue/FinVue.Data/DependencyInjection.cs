using FinVue.Core.Interfaces;
﻿using FinVue.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinVue.Data; 

public static class DependencyInjection {

    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration) {

        services.AddDbContext<ApplicationDbContext>(opt => {
            opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        return services;
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