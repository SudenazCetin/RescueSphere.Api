namespace RescueSphere.Api.DTOs.HelpRequests
{
    public class HelpRequestUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }

        public string? Status { get; set; }
        public string? Priority { get; set; }
    }
}
