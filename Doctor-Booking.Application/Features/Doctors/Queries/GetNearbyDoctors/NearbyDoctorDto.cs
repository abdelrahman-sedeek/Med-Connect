namespace Doctor_Booking.Application.Features.Doctors.Queries.GetNearbyDoctors
{
    public record NearbyDoctorDto
    {
        public int Id { get; init; }
        public string FullName { get; init; } = string.Empty;
        public string Specialty { get; init; } = string.Empty;
        public string ClinicName { get; init; } = string.Empty;
        
        private decimal _rating;
        public decimal Rating
        {
            get => _rating;
            init => _rating = Math.Round(value, 1);
        }
        public string ImageUrl { get; init; } = string.Empty;
        public DateTime AvailableStartDate { get; init; }
        public DateTime AvailableEndDate { get; init; }
    }
}
