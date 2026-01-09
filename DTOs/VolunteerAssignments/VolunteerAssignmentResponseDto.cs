namespace RescueSphere.Api.DTOs.VolunteerAssignments;

public class VolunteerAssignmentResponseDto
{
    public int Id { get; set; }
    public int HelpRequestId { get; set; }
    public int VolunteerUserId { get; set; }
    public string Status { get; set; } = null!;
    public DateTime AssignedAt { get; set; }
}
