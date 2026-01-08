using Microsoft.EntityFrameworkCore;
using RescueSphere.Api.Data;
using RescueSphere.Api.Services.Interfaces;
using RescueSphere.Api.Services.Implementations;
using RescueSphere.Api.Common;
using RescueSphere.Api.DTOs.HelpRequests;
using RescueSphere.Api.Services.Interfaces;


// DTOs
using RescueSphere.Api.DTOs.Users;
using RescueSphere.Api.DTOs.Categories;
using RescueSphere.Api.DTOs.HelpRequests;

var builder = WebApplication.CreateBuilder(args);

// ================== SERVICES ==================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=rescueSphere.db"));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISupportCategoryService, SupportCategoryService>();
builder.Services.AddScoped<IHelpRequestService, HelpRequestService>();

// ================== SWAGGER ==================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "RescueSphere API",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RescueSphere API v1");
});


// ================= USERS =================

// CREATE
app.MapPost("/users", async (CreateUserDto dto, IUserService service) =>
{
    var created = await service.CreateUserAsync(dto);
    return Results.Created($"/users/{created.Id}",
        ApiResponse<UserResponseDto>.Ok(created, "User created successfully"));
});

// GET ALL
app.MapGet("/users", async (IUserService service) =>
{
    var users = await service.GetAllAsync();
    return Results.Ok(ApiResponse<List<UserResponseDto>>.Ok(users));
});

// GET BY ID
app.MapGet("/users/{id:int}", async (int id, IUserService service) =>
{
    var user = await service.GetByIdAsync(id);
    if (user is null)
        return Results.NotFound(ApiResponse<string>.Fail("User not found"));

    return Results.Ok(ApiResponse<UserResponseDto>.Ok(user));
});

// UPDATE
app.MapPut("/users/{id:int}", async (int id, UpdateUserDto dto, IUserService service) =>
{
    var updated = await service.UpdateAsync(id, dto);
    if (updated is null)
        return Results.NotFound(ApiResponse<string>.Fail("User not found"));

    return Results.Ok(ApiResponse<UserResponseDto>.Ok(updated, "User updated successfully"));
});

// DELETE
app.MapDelete("/users/{id:int}", async (int id, IUserService service) =>
{
    var result = await service.SoftDeleteAsync(id);
    if (!result)
        return Results.NotFound(ApiResponse<string>.Fail("User not found"));

    return Results.Ok(ApiResponse<string>.Ok(null, "User deleted successfully"));
});


// ================= CATEGORIES =================

app.MapPost("/categories", async (SupportCategoryCreateDto dto, ISupportCategoryService service) =>
{
    var created = await service.CreateAsync(dto);
    return Results.Created($"/categories/{created.Id}",
        ApiResponse<SupportCategoryResponseDto>.Ok(created, "Category created"));
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
        return Results.NotFound(ApiResponse<string>.Fail("Category not found"));

    return Results.Ok(ApiResponse<SupportCategoryResponseDto>.Ok(category));
});

app.MapPut("/categories/{id:int}", async (int id, SupportCategoryUpdateDto dto, ISupportCategoryService service) =>
{
    var updated = await service.UpdateAsync(id, dto);
    if (updated is null)
        return Results.NotFound(ApiResponse<string>.Fail("Category not found"));

    return Results.Ok(ApiResponse<SupportCategoryResponseDto>.Ok(updated, "Category updated"));
});

app.MapDelete("/categories/{id:int}", async (int id, ISupportCategoryService service) =>
{
    var deleted = await service.SoftDeleteAsync(id);
    if (!deleted)
        return Results.NotFound(ApiResponse<string>.Fail("Category not found"));

    return Results.Ok(ApiResponse<string>.Ok(null, "Category deleted"));
});
// ================= HELP REQUEST ENDPOINTS =================

app.MapPost("/help-requests", async (
    HelpRequestCreateDto dto,
    IHelpRequestService service) =>
{
    var created = await service.CreateAsync(dto);
    return Results.Created(
        $"/help-requests/{created.Id}",
        ApiResponse<HelpRequestResponseDto>.Ok(created, "Help request created"));
});


app.MapGet("/help-requests", async (IHelpRequestService service) =>
{
    var list = await service.GetAllAsync();
    return Results.Ok(ApiResponse<List<HelpRequestResponseDto>>.Ok(list));
});


app.MapGet("/help-requests/{id:int}", async (int id, IHelpRequestService service) =>
{
    var item = await service.GetByIdAsync(id);
    if (item is null)
        return Results.NotFound(
            ApiResponse<HelpRequestResponseDto>.Fail("Help request not found"));

    return Results.Ok(ApiResponse<HelpRequestResponseDto>.Ok(item));
});


app.MapPut("/help-requests/{id:int}", async (
    int id,
    HelpRequestUpdateDto dto,
    IHelpRequestService service) =>
{
    var updated = await service.UpdateAsync(id, dto);
    if (updated is null)
        return Results.NotFound(
            ApiResponse<HelpRequestResponseDto>.Fail("Help request not found"));

    return Results.Ok(
        ApiResponse<HelpRequestResponseDto>.Ok(updated, "Help request updated"));
});


app.MapDelete("/help-requests/{id:int}", async (int id, IHelpRequestService service) =>
{
    var result = await service.SoftDeleteAsync(id);
    if (!result)
        return Results.NotFound(
            ApiResponse<string>.Fail("Help request not found"));

    return Results.Ok(ApiResponse<string>.Ok(null, "Help request deleted"));
});




// ================= ROOT =================
app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

app.Run();
