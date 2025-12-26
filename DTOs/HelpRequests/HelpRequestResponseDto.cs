namespace RescueSphere.Api.DTOs.HelpRequests
{
    public class HelpRequestResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Location { get; set; }

        public string Status { get; set; } = null!;
        public string Priority { get; set; } = null!;

        public int RequestedByUserId { get; set; }
        public string RequestedByUsername { get; set; } = null!;

        public int SupportCategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
