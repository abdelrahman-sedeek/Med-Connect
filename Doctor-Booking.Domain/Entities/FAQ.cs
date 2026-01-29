using Doctor_Booking.Domain.Common;

namespace Doctor_Booking.Domain.Entities
{
    public class FAQ: BaseEntity
    {
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
