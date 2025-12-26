namespace RescueSphere.Api.Domain.Entities;

public enum HelpRequestStatus
{
    Pending,
    InProgress,
    Resolved,
    Cancelled
}

public enum HelpRequestPriority
{
    Low,
    Medium,
    High,
    Critical
}

public class HelpRequest : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Location { get; set; }

    public HelpRequestStatus Status { get; set; } = HelpRequestStatus.Pending;
    public HelpRequestPriority Priority { get; set; } = HelpRequestPriority.Medium;

    public int RequestedByUserId { get; set; }
    public User RequestedByUser { get; set; } = null!;

    public int SupportCategoryId { get; set; }
    public SupportCategory SupportCategory { get; set; } = null!;

    public ICollection<VolunteerAssignment> VolunteerAssignments { get; set; } = new List<VolunteerAssignment>();
}
