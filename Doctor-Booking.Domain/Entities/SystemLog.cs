using Doctor_Booking.Domain.Common;

namespace Doctor_Booking.Domain.Entities
{
    public class SystemLog : BaseEntity
    {
        public string Action { get; set; } = string.Empty;
    }
}
