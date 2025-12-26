using Microsoft.EntityFrameworkCore;
using RescueSphere.Api.Data;
using RescueSphere.Api.Services.Interfaces;
using RescueSphere.Api.Services.Implementations;
using RescueSphere.Api.Common;
using RescueSphere.Api.DTOs.Users;
using RescueSphere.Api.DTOs.Categories;
using RescueSphere.Api.Services.Interfaces;
using RescueSphere.Api.Common;

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
builder.Services.AddScoped<ISupportCategoryService, SupportCategoryService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RescueSphere API v1");
});

// ================= USERS ENDPOINTS =================

// CREATE
app.MapPost("/users", async (CreateUserDto dto, IUserService userService) =>
{
    var created = await userService.CreateUserAsync(dto);
    return Results.Created($"/users/{created.Id}",
        ApiResponse<UserResponseDto>.Ok(created, "User created successfully"));
});

// GET ALL
app.MapGet("/users", async (IUserService userService) =>
{
    var users = await userService.GetAllAsync();
    return Results.Ok(ApiResponse<List<UserResponseDto>>.Ok(users));
});

// GET BY ID
app.MapGet("/users/{id:int}", async (int id, IUserService userService) =>
{
    var user = await userService.GetByIdAsync(id);
    if (user is null)
        return Results.NotFound(ApiResponse<UserResponseDto>.Fail("User not found"));

    return Results.Ok(ApiResponse<UserResponseDto>.Ok(user));
});

// UPDATE
app.MapPut("/users/{id:int}", async (int id, UpdateUserDto dto, IUserService userService) =>
{
    var updated = await userService.UpdateAsync(id, dto);

    if (updated is null)
        return Results.NotFound(ApiResponse<string>.Fail("User not found"));

    return Results.Ok(ApiResponse<UserResponseDto>.Ok(updated, "User updated successfully"));
});

// SOFT DELETE
app.MapDelete("/users/{id:int}", async (int id, IUserService userService) =>
{
    var result = await userService.SoftDeleteAsync(id);

    if (!result)
        return Results.NotFound(ApiResponse<string>.Fail("User not found"));

    return Results.Ok(ApiResponse<string>.Ok(null, "User deleted successfully"));
});
// ================= CATEGORY ENDPOINTS =================

app.MapPost("/categories", async (SupportCategoryCreateDto dto, ISupportCategoryService service) =>
{
    var created = await service.CreateAsync(dto);
    return Results.Created($"/categories/{created.Id}", ApiResponse<SupportCategoryResponseDto>.Ok(created, "Category created"));
});

app.MapGet("/categories", async (ISupportCategoryService service) =>
{
    var list = await service.GetAllAsync();
    return Results.Ok(ApiResponse<List<SupportCategoryResponseDto>>.Ok(list));
});

app.MapGet("/categories/{id:int}", async (int id, ISupportCategoryService service) =>
{
    var category = await service.GetByIdAsync(id);
    if (category is null)
        return Results.NotFound(ApiResponse<SupportCategoryResponseDto>.Fail("Category not found"));

    return Results.Ok(ApiResponse<SupportCategoryResponseDto>.Ok(category));
});

app.MapPut("/categories/{id:int}", async (int id, SupportCategoryUpdateDto dto, ISupportCategoryService service) =>
{
    var updated = await service.UpdateAsync(id, dto);
    if (updated is null)
        return Results.NotFound(ApiResponse<SupportCategoryResponseDto>.Fail("Category not found"));

    return Results.Ok(ApiResponse<SupportCategoryResponseDto>.Ok(updated, "Category updated"));
});

app.MapDelete("/categories/{id:int}", async (int id, ISupportCategoryService service) =>
{
    var result = await service.SoftDeleteAsync(id);
    if (!result)
        return Results.NotFound(ApiResponse<string>.Fail("Category not found"));

    return Results.Ok(ApiResponse<string>.Ok(null, "Category deleted"));
});
// ===================================================

app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

app.Run();
