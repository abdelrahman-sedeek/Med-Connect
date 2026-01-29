namespace Doctor_Booking.Application.DTOs
{
    public record AvailabilitySlotsDto
    {
        public int Id { get; init; }
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public bool IsBooked { get; init; }
        public int DoctorId { get; init; }
    }
}
