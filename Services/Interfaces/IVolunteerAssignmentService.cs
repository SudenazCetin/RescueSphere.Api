using RescueSphere.Api.DTOs.VolunteerAssignments;

public interface IVolunteerAssignmentService
{
    Task<bool> AssignAsync(VolunteerAssignmentCreateDto dto);
Task<List<VolunteerAssignmentResponseDto>> GetAllAsync();
Task<VolunteerAssignmentResponseDto?> GetByIdAsync(int id);

}
