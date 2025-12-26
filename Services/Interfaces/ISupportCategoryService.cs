using RescueSphere.Api.DTOs.Categories;

namespace RescueSphere.Api.Services.Interfaces
{
    public interface ISupportCategoryService
    {
        Task<SupportCategoryResponseDto> CreateAsync(SupportCategoryCreateDto dto);
        Task<List<SupportCategoryResponseDto>> GetAllAsync();
        Task<SupportCategoryResponseDto?> GetByIdAsync(int id);
        Task<SupportCategoryResponseDto?> UpdateAsync(int id, SupportCategoryUpdateDto dto);
        Task<bool> SoftDeleteAsync(int id);
    }
}
