namespace RescueSphere.Api.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public UserRole Role { get; set; } = UserRole.Citizen;

    public ICollection<HelpRequest> HelpRequests { get; set; } = new List<HelpRequest>();
    public ICollection<VolunteerAssignment> VolunteerAssignments { get; set; } = new List<VolunteerAssignment>();
}
