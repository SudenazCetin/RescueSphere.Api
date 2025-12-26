namespace RescueSphere.Api.DTOs.HelpRequests
{
    public class HelpRequestCreateDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Location { get; set; }

        public int RequestedByUserId { get; set; }
        public int SupportCategoryId { get; set; }

        public string Priority { get; set; } = "Medium";
    }
}
