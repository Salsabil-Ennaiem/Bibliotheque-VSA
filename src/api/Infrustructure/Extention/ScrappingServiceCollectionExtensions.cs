using Data;
using domain.Interfaces;
using Features.ScrapingLivres;
using Infrastructure.Repositries;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extentions;

public static class ScrapingServiceCollectionExtensions{
    public static IServiceCollection AddBiruniServices(this IServiceCollection services, IConfiguration config)
    {

        services.AddDbContext<BiblioDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("postgres")));


        services.AddHttpClient<BiruniHtmlExtractor>(client =>
        {
            client.BaseAddress = new Uri("https://www.biruni.tn/catalogue-local.php?ei=108/");
            client.DefaultRequestHeaders.UserAgent.ParseAdd("BiruniScraper/1.0");
        });

        services.AddScoped<IScrapingRepository, ScrapingRepository>();
        services.AddScoped<BookDataTransformer>();
        services.AddScoped<ScrapingPipeline>();

        return services;
    }}