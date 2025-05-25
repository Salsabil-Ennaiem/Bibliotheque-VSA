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

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BiblioDbContext>(options => 
        options.UseNpgsql(builder.Configuration.GetConnectionString("postgres")).EnableSensitiveDataLogging());



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

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ILivresRepository, LivresRepository>();
builder.Services.AddScoped<IEmpruntsRepository, EmpruntsRepository>();
// builder.Services.AddScoped<IParametreRepository, ParametreRepository>();
builder.Services.AddScoped<ISanctionRepository, SanctionRepository>();
builder.Services.AddScoped<IScrapingRepository, ScrapingRepository>();
builder.Services.AddScoped<INouveauteRepository , NouveauteRepository>();

builder.Services.AddScoped<LivresHandler>();
builder.Services.AddScoped<EmpruntHandler>();
//builder.Services.AddScoped<ParametreHandler>();
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
