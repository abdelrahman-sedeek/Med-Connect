namespace Doctor_Booking.Application.Interfaces;

public interface INotificationPublisher
{
	Task PublishAsync(
		int userId,
		object payload,
		CancellationToken cancellationToken);
}
