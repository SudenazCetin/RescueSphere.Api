using Microsoft.EntityFrameworkCore;
using RescueSphere.Api.Data;
using RescueSphere.Api.Domain.Entities;
using RescueSphere.Api.DTOs.Categories;
using RescueSphere.Api.Services.Interfaces;

namespace RescueSphere.Api.Services.Implementations
{
    public class SupportCategoryService : ISupportCategoryService
    {
        private readonly AppDbContext _context;

        public SupportCategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SupportCategoryResponseDto> CreateAsync(SupportCategoryCreateDto dto)
        {
            var category = new SupportCategory
            {
                Name = dto.Name
            };

            _context.SupportCategories.Add(category);
            await _context.SaveChangesAsync();

            return MapToResponse(category);
        }

        public async Task<List<SupportCategoryResponseDto>> GetAllAsync()
        {
            var list = await _context.SupportCategories.AsNoTracking().ToListAsync();
            return list.Select(MapToResponse).ToList();
        }

        public async Task<SupportCategoryResponseDto?> GetByIdAsync(int id)
        {
            var category = await _context.SupportCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return category is null ? null : MapToResponse(category);
        }

        public async Task<SupportCategoryResponseDto?> UpdateAsync(int id, SupportCategoryUpdateDto dto)
        {
            var category = await _context.SupportCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null) return null;

            category.Name = dto.Name;
            category.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToResponse(category);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var category = await _context.SupportCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null) return false;

            category.IsDeleted = true;
            category.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        private static SupportCategoryResponseDto MapToResponse(SupportCategory c)
            => new()
            {
                Id = c.Id,
                Name = c.Name,
                CreatedAt = c.CreatedAt
            };
    }
}
