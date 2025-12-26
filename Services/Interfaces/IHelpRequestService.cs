using RescueSphere.Api.DTOs.HelpRequests;

namespace RescueSphere.Api.Services.Interfaces
{
    public interface IHelpRequestService
    {
        Task<HelpRequestResponseDto> CreateAsync(HelpRequestCreateDto dto);
        Task<List<HelpRequestResponseDto>> GetAllAsync();
        Task<HelpRequestResponseDto?> GetByIdAsync(int id);
        Task<HelpRequestResponseDto?> UpdateAsync(int id, HelpRequestUpdateDto dto);
        Task<bool> SoftDeleteAsync(int id);
    }
}
