using Doctor_Booking.Domain.Common;
using Doctor_Booking.Domain.Enum;

namespace Doctor_Booking.Domain.Entities
{
    public class Payment: BaseEntity
    {
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string TransactionRef { get; set; } = string.Empty;

        public int? BookingId { get; set; }
        public int? PaymentMethodId { get; set; }

        // Navigation
        public Booking? Booking { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
    }
}
