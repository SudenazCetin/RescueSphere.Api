using Microsoft.EntityFrameworkCore;
using RescueSphere.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Swagger (OpenAPI yerine bunu kullanacağız)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=rescueSphere.db"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapGet("/", () => "RescueSphere API is running...");

app.Run();
