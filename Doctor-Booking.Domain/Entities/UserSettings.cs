using Doctor_Booking.Domain.Common;

namespace Doctor_Booking.Domain.Entities;

public class UserSettings : BaseEntity
{
	public int UserId { get; set; }
	public bool NotificationsEnabled { get; set; } = true;

	// Navigation
	public ApplicationUser User { get; set; } = null!;
}

