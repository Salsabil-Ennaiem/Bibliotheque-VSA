using Microsoft.EntityFrameworkCore;
using Data;
//using Seeders;
using domain.Entity;


namespace Infrastructure.Extentions;

public static class ServiceICollectionExtentions
{
        public static IServiceCollection AddPersistenceServices( this IServiceCollection services, IConfiguration config)
{

    services.AddDbContext<BiblioDbContext>(options => 
        options.UseNpgsql(config.GetConnectionString("postgres")).EnableSensitiveDataLogging());
        

    services.AddIdentityCore<Bibliothecaire>(options => 
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
        })
        .AddEntityFrameworkStores<BiblioDbContext>();

    
    //services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

    return services;
}

}
