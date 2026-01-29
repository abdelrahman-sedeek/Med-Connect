using Doctor_Booking.Domain.Common;
using Doctor_Booking.Domain.Enum;

namespace Doctor_Booking.Domain.Entities
{
    public class Booking: BaseEntity
    {
        public BookingStatus Status { get; set; } = BookingStatus.Pending ;

        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }
        public int? AvailabilitySlotId { get; set; }
        
        // Navigation     
        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
        public AvailabilitySlot? AvailabilitySlot { get; set; }
        public Payment? Payment { get; set; }
        public Review? Review { get; set; }
    }
}
