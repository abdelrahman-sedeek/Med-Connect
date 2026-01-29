using Microsoft.AspNetCore.SignalR;

namespace DoctorBooking.Hubs;

public class SignalRNotificationPublisher : INotificationPublisher
{
	private readonly IHubContext<NotificationHub> _hubContext;

	public SignalRNotificationPublisher(
		IHubContext<NotificationHub> hubContext)
	{
		_hubContext = hubContext;
	}

	public async Task PublishAsync(
		int userId,
		object payload,
		CancellationToken cancellationToken)
	{
		await _hubContext.Clients
			.Group(userId.ToString())
			.SendAsync("ReceiveNotification", payload, cancellationToken);
	}
}
