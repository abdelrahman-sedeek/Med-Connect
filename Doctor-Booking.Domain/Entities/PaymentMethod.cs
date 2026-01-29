using Doctor_Booking.Domain.Common;

namespace Doctor_Booking.Domain.Entities
{
    public class PaymentMethod:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    }
}
