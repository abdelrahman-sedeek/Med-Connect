using Doctor_Booking.Domain.Common;

namespace Doctor_Booking.Domain.Entities
{
    public class Favorite: BaseEntity
    {
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }

        // Navigation
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
