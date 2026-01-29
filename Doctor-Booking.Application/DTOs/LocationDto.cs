namespace Doctor_Booking.Application.DTOs
{
    public record LocationDto
    {
        public double Latitude { get; init; }
        public double Longitude { get; init; }
    }
}
