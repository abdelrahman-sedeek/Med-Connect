using Doctor_Booking.Application.Common.Notifications;

namespace Doctor_Booking.Application.Interfaces.Services;

public interface INotificationService
{
	Task CreateAsync(
		NotificationMessage message,
		CancellationToken cancellationToken);
}
