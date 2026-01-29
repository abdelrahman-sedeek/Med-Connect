using Doctor_Booking.Domain.Common;
using Doctor_Booking.Domain.Enum;
using NetTopologySuite.Geometries;

namespace Doctor_Booking.Domain.Entities
{
    public class Doctor: BaseEntity
    {
        public string About { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string ClinicName { get; set; } = string.Empty;
        public Point Location { get; set; } = Point.Empty;
        public decimal SessionPrice { get; set; }
        public string? TempPasswordHash { get; set; }
        public bool IsDoctorApproved { get; set; } = false;
        public bool IsChangedPassword { get; set; } = false;
		public DoctorStatus Status { get; set; } = DoctorStatus.Verifiied;


		public int? UserId { get; set; }
        public int? SpecialtyId { get; set; }

        // Navigation
        public ApplicationUser? User { get; set; }
        public Specialty? Specialty { get; set; }
        public ICollection<SearchHistory> SearchHistories { get; set; } = new HashSet<SearchHistory>();
        public ICollection<AvailabilitySlot> AvailabilitySlots { get; set; } = new HashSet<AvailabilitySlot>();
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    }
}
