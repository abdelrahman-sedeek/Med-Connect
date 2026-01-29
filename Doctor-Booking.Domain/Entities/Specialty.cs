using Doctor_Booking.Domain.Common;

namespace Doctor_Booking.Domain.Entities
{
    public class Specialty : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();
    }
}
