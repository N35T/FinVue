using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinVue.Data; 

public static class DependencyInjection {

    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration) {

        services.AddDbContext<ApplicationDbContext>(opt => {
            opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}