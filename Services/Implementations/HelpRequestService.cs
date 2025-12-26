using Microsoft.EntityFrameworkCore;
using RescueSphere.Api.Common;
using RescueSphere.Api.Data;
using RescueSphere.Api.Domain.Entities;
using RescueSphere.Api.DTOs.HelpRequests;
using RescueSphere.Api.Services.Interfaces;

namespace RescueSphere.Api.Services.Implementations
{
    public class HelpRequestService : IHelpRequestService
    {
        private readonly AppDbContext _context;

        public HelpRequestService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<HelpRequestResponseDto> CreateAsync(HelpRequestCreateDto dto)
        {
            var entity = new HelpRequest
            {
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                RequestedByUserId = dto.RequestedByUserId,
                SupportCategoryId = dto.SupportCategoryId,
                Priority = Enum.Parse<HelpRequestPriority>(dto.Priority, true)
            };

            _context.HelpRequests.Add(entity);
            await _context.SaveChangesAsync();

            return await MapToResponse(entity.Id);
        }

        public async Task<List<HelpRequestResponseDto>> GetAllAsync()
        {
            return await _context.HelpRequests
                .Include(x => x.RequestedByUser)
                .Include(x => x.SupportCategory)
                .Select(x => new HelpRequestResponseDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Location = x.Location,
                    Status = x.Status.ToString(),
                    Priority = x.Priority.ToString(),
                    RequestedByUserId = x.RequestedByUserId,
                    RequestedByUsername = x.RequestedByUser.Username,
                    SupportCategoryId = x.SupportCategoryId,
                    CategoryName = x.SupportCategory.Name,
                    CreatedAt = x.CreatedAt
                }).ToListAsync();
        }

        public async Task<HelpRequestResponseDto?> GetByIdAsync(int id)
        {
            var request = await _context.HelpRequests
                .Include(x => x.RequestedByUser)
                .Include(x => x.SupportCategory)
                .FirstOrDefaultAsync(x => x.Id == id);

            return request == null ? null : await MapToResponse(request.Id);
        }

        public async Task<HelpRequestResponseDto?> UpdateAsync(int id, HelpRequestUpdateDto dto)
        {
            var entity = await _context.HelpRequests.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return null;

            if (!string.IsNullOrWhiteSpace(dto.Title))
                entity.Title = dto.Title;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                entity.Description = dto.Description;

            if (!string.IsNullOrWhiteSpace(dto.Location))
                entity.Location = dto.Location;

            if (!string.IsNullOrWhiteSpace(dto.Status))
                entity.Status = Enum.Parse<HelpRequestStatus>(dto.Status, true);

            if (!string.IsNullOrWhiteSpace(dto.Priority))
                entity.Priority = Enum.Parse<HelpRequestPriority>(dto.Priority, true);

            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await MapToResponse(entity.Id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var entity = await _context.HelpRequests.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<HelpRequestResponseDto> MapToResponse(int id)
        {
            var x = await _context.HelpRequests
                .Include(r => r.RequestedByUser)
                .Include(r => r.SupportCategory)
                .FirstAsync(r => r.Id == id);

            return new HelpRequestResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Location = x.Location,
                Status = x.Status.ToString(),
                Priority = x.Priority.ToString(),
                RequestedByUserId = x.RequestedByUserId,
                RequestedByUsername = x.RequestedByUser.Username,
                SupportCategoryId = x.SupportCategoryId,
                CategoryName = x.SupportCategory.Name,
                CreatedAt = x.CreatedAt
            };
        }
    }
}
