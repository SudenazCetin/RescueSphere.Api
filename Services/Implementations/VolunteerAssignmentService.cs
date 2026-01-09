using Microsoft.EntityFrameworkCore;
using RescueSphere.Api.Data;
using RescueSphere.Api.Domain.Entities;
using RescueSphere.Api.DTOs.VolunteerAssignments;
using RescueSphere.Api.Services.Interfaces;

namespace RescueSphere.Api.Services.Implementations;

public class VolunteerAssignmentService : IVolunteerAssignmentService
{
    private readonly AppDbContext _context;

    public VolunteerAssignmentService(AppDbContext context)
    {
        _context = context;
    }

    // ASSIGN (POST)
    public async Task<bool> AssignAsync(VolunteerAssignmentCreateDto dto)
    {
        var helpRequestExists = await _context.HelpRequests
            .AnyAsync(x => x.Id == dto.HelpRequestId);

        if (!helpRequestExists)
            return false;

        var assignment = new VolunteerAssignment
        {
            VolunteerUserId = dto.VolunteerUserId,
            HelpRequestId = dto.HelpRequestId,
            Status = AssignmentStatus.Assigned,
            AssignedAt = DateTime.UtcNow
        };

        _context.VolunteerAssignments.Add(assignment);
        await _context.SaveChangesAsync();

        return true;
    }

    // GET ALL
    public async Task<List<VolunteerAssignmentResponseDto>> GetAllAsync()
    {
        var list = await _context.VolunteerAssignments
            .AsNoTracking()
            .ToListAsync();

        return list.Select(MapToResponse).ToList();
    }

    // GET BY ID
    public async Task<VolunteerAssignmentResponseDto?> GetByIdAsync(int id)
    {
        var entity = await _context.VolunteerAssignments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity is null ? null : MapToResponse(entity);
    }

    // MAP
    private static VolunteerAssignmentResponseDto MapToResponse(VolunteerAssignment entity)
    {
        return new VolunteerAssignmentResponseDto
        {
            Id = entity.Id,
            HelpRequestId = entity.HelpRequestId,
            VolunteerUserId = entity.VolunteerUserId,
            Status = entity.Status.ToString(),
            AssignedAt = entity.AssignedAt
        };
    }
}
