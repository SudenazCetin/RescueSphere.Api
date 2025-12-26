using Microsoft.EntityFrameworkCore;
using RescueSphere.Api.Data;
using RescueSphere.Api.Services.Interfaces;
using RescueSphere.Api.Services.Implementations;
using RescueSphere.Api.Common;
using RescueSphere.Api.DTOs.Users;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "RescueSphere API", 
        Version = "v1" 
    });
});


// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=rescueSphere.db"));

// Services
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RescueSphere API v1");
});

 
// ================= USERS ENDPOINTS =================

app.MapPost("/users", async (CreateUserDto dto, IUserService userService) =>
{
    var created = await userService.CreateUserAsync(dto);
    return Results.Created($"/users/{created.Id}",
        ApiResponse<UserResponseDto>.Ok(created, "User created successfully"));
});

app.MapGet("/users", async (IUserService userService) =>
{
    var users = await userService.GetAllAsync();
    return Results.Ok(ApiResponse<List<UserResponseDto>>.Ok(users));
});

app.MapGet("/users/{id:int}", async (int id, IUserService userService) =>
{
    var user = await userService.GetByIdAsync(id);
    if (user is null)
        return Results.NotFound(ApiResponse<UserResponseDto>.Fail("User not found"));

    return Results.Ok(ApiResponse<UserResponseDto>.Ok(user));
});

// ===================================================

app.MapGet("/", () => Results.Redirect("/swagger/index.html"));


app.Run();
