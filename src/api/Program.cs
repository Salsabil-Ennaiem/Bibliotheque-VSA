using Infrastructure.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistenceServices(builder.Configuration);


builder.Services.AddBiruniServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>{
    c.SwaggerDoc("v1", new() { Title = "Biruni Scraper", Version = "v1" });});


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
//app.MapIdentityApi<Bibliothecaire>();

app.MapControllers();

app.Run();
