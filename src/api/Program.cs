using Data;
using domain.Entity;
using presistence.Extentions;
//using Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistenceServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.CreateScope();
   // await scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>().SeedAsync(scope.ServiceProvider.GetRequiredService<BiblioDbContext>());
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapIdentityApi<Bibliothecaire>();

app.MapControllers();

app.Run();
