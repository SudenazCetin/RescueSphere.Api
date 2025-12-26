namespace RescueSphere.Api.Domain.Entities;

public enum AssignmentStatus
{
    Assigned,
    InProgress,
    Completed,
    Cancelled
}

public class VolunteerAssignment : BaseEntity
{
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public AssignmentStatus Status { get; set; } = AssignmentStatus.Assigned;

    public int HelpRequestId { get; set; }
    public HelpRequest HelpRequest { get; set; } = null!;

    public int VolunteerUserId { get; set; }
    public User VolunteerUser { get; set; } = null!;
}
