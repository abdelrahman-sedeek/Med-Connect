using Doctor_Booking.Domain.Common;

namespace Doctor_Booking.Domain.Entities
{
    public class ChatMessage: BaseEntity
    {
        public string Message { get; set; } = string.Empty;
        public string ContentType { get; set; } = "Text";
        public bool IsRead { get; set; }

        public int? ChatId { get; set; }
        public int? SenderUserId { get; set; }

        // Navigation
        public Chat? Chat { get; set; }
        public ApplicationUser? Sender { get; set; }
    }
}
