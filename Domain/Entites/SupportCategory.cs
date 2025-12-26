namespace RescueSphere.Api.Domain.Entities;

public class SupportCategory : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<HelpRequest> HelpRequests { get; set; } = new List<HelpRequest>();
}
