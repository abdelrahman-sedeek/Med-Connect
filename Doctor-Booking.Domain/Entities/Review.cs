using Doctor_Booking.Domain.Common;

namespace Doctor_Booking.Domain.Entities
{
    public class Review:BaseEntity
    {
        public decimal Rating { get; set; }
        public string Comment { get; set; } = string.Empty;

        public int? BookingId { get; set; }
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }

        // Navigation
        public Booking? Booking { get; set; }
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
