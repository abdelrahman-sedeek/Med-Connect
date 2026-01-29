using Doctor_Booking.Domain.Common;

namespace Doctor_Booking.Domain.Entities
{
    public class AvailabilitySlot: BaseEntity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsBooked { get; set; } = false;

        public int? DoctorId { get; set; }

        // Navigation
        public Doctor? Doctor { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}
