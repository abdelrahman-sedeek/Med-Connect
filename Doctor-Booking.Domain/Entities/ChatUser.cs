namespace Doctor_Booking.Domain.Entities
{
    public class ChatUser 
    {
        public int? ChatId { get; set; }
        public int? UserId { get; set; }
        public string status { get; set; } = "normal";

        // Navigations
        public Chat? Chat { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
