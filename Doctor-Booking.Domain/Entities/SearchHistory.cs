using Doctor_Booking.Domain.Common;

namespace Doctor_Booking.Domain.Entities
{
    public class SearchHistory : BaseEntity
    {
        public string Keyword { get; set; } = string.Empty;
        public string? Specialty { get; set; }
        public string? Location { get; set; }

        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }

        // Navigation
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
