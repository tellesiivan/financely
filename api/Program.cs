using api.data;
using api.services.Stock;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// set up db context with connection string using sql server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{ 
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddScoped<IStockService, StockService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// must be above UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
