using System.Text;
using api.Features.Auth.Login;
using api.Features.ScrapingLivres;
using api.Features.Auth;
using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using domain.Entity;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Seeders;
using System.Threading.RateLimiting;
using domain.Interfaces;
using Infrastructure.Repositries;
using api.Features.Livre;
using api.Features.Nouveautes;
using api.Features.Emprunt;
using api.Features.Parametre;
using api.Features.Sanction;
using Npgsql;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("postgres"));
dataSourceBuilder.EnableDynamicJson();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<BiblioDbContext>(options =>
{
    options.UseNpgsql(dataSource);
});

/*
builder.Services.AddDbContext<BiblioDbContext>(options => 
        options.UseNpgsql(builder.Configuration.GetConnectionString("postgres")).EnableSensitiveDataLogging());
*/


builder.Services.AddIdentity<Bibliothecaire, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; 
    options.SignIn.RequireConfirmedPhoneNumber = false;
}).AddEntityFrameworkStores<BiblioDbContext>() .AddDefaultTokenProviders();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        ValidAudience = builder.Configuration["Jwt:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
    };
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddHttpContextAccessor();


builder.Services.AddCors(options => {
    options.AddPolicy("Angular", policy => 
        policy.WithOrigins("https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("login", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(5)
            }));
});

builder.Services.AddScoped<IRepository<Nouveaute>, Repository<Nouveaute>>();
builder.Services.AddScoped<IRepository<Emprunts>, Repository<Emprunts>>();
builder.Services.AddScoped<IRepository<Parametre>, Repository<Parametre>>();
builder.Services.AddScoped<IRepository<Sanction>, Repository<Sanction>>();
builder.Services.AddScoped<IRepository<Livres>, Repository<Livres>>();

builder.Services.AddScoped<Repository<Parametre>>();
builder.Services.AddScoped<Repository<Sanction>>();

builder.Services.AddScoped<ILivresRepository, LivresRepository>();
builder.Services.AddScoped<IEmpruntsRepository, EmpruntsRepository>();
builder.Services.AddScoped<IParametreRepository, ParametreRepository>();
builder.Services.AddScoped<IScrapingRepository, ScrapingRepository>();
builder.Services.AddScoped<ISanctionRepository, SanctionRepository>();
builder.Services.AddScoped<INouveauteRepository , NouveauteRepository>();
 
builder.Services.AddScoped<LivresHandler>();
builder.Services.AddScoped<EmpruntHandler>();
builder.Services.AddScoped<ParametreHandler>();
builder.Services.AddScoped<SanctionHandler>();
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<NouveauteHandler>();


builder.Services.AddBiruniServices(builder.Configuration);
builder.Services.AddHttpClient<BiruniHtmlExtractor>(client =>
{
    client.BaseAddress = new Uri("https://www.biruni.tn/catalogue-local.php?ei=108/");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("BiruniScraper/1.0");
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>{
    c.SwaggerDoc("v1", new() { Title = "Biruni Scraper", Version = "v1" });});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DataSeeder.SeedUsersAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during seeding: {ex.Message}");
    }
}

app.UseCors("Angular");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.UseRateLimiter();
app.Run();
