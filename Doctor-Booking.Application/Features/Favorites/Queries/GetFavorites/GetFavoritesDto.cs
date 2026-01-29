namespace Doctor_Booking.Application.Features.Favorites.Queries.GetFavorites
{
    public record GetFavoritesDto
    {
        public int? DoctorId { get; init; }
        // form ApplicationUser Table (firstName, lastName)
        public string FullName { get; init; } = string.Empty;
        // from Specialty Table (Name)
        public string Specialty { get; init; } = string.Empty;
        public string ClinicName { get; init; } = string.Empty;
        // from Review Table (Rating)
        private decimal _rating;
        public decimal Rating
        {
            get => _rating;
            init => _rating = Math.Round(value, 1);
        }
        public string ImageUrl { get; init; } = string.Empty;
        // from AvailabilitySlot Table (StartTime)
        public DateTime AvailableStartDate { get; init; }
        // from AvailabilitySlot Table (EndTime)
        public DateTime AvailableEndDate { get; init; }
    }
}
