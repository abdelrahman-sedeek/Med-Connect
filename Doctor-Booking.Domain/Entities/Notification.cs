using Doctor_Booking.Domain.Common;

namespace Doctor_Booking.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        
        public int? UserId { get; set; }

        // Navigation
        public ApplicationUser? User { get; set; }
    }
}
