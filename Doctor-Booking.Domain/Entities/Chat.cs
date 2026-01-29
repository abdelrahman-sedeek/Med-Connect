using Doctor_Booking.Domain.Common;

namespace Doctor_Booking.Domain.Entities
{
    public class Chat: BaseEntity
    {
        public ICollection<ChatUser> ChatUsers { get; set; } = new HashSet<ChatUser>();
        public ICollection<ChatMessage> Messages { get; set; } = new HashSet<ChatMessage>();
    }
}
