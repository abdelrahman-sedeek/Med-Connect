namespace Doctor_Booking.Application.Features.SearchHistory.Queries.GetSearchHistory
{
    public record GetSearchHistoryDto
    {
        public int Id { get; init; }
        public string? Query { get; init; }
        public string? Specialty { get; init; }
        public string? Location { get; init; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
