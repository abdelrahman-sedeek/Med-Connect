using Doctor_Booking.Application.Common.Notifications;
using Doctor_Booking.Application.Interfaces.Services;
using Doctor_Booking.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

namespace Doctor_Booking.Infastructure.Notification;


public class NotificationService : INotificationService
{
	private readonly IDoctorBookingDbContext _context;
	private readonly INotificationPublisher _publisher;

	public NotificationService(
		IDoctorBookingDbContext context,
		INotificationPublisher publisher)
	{
		_context = context;
		_publisher = publisher;
	}

	public async Task CreateAsync(
		NotificationMessage message,
		CancellationToken cancellationToken)
	{
		var settings = await _context.UserSettings
			.FirstOrDefaultAsync(x => x.UserId == message.UserId, cancellationToken);

		if (settings != null && !settings.NotificationsEnabled)
			return;

		var notification = new Domain.Entities.Notification
		{
			UserId = message.UserId,
			Title = message.Title,
			Message = message.Body
		};

		_context.Notifications.Add(notification);
		await _context.SaveChangesAsync(cancellationToken);

		// 👇 Infrastructure مش عارف SignalR
		await _publisher.PublishAsync(
			message.UserId,
			new
			{
				notification.Id,
				notification.Title,
				notification.Message,
				notification.CreatedAt
			},
			cancellationToken);
	}
}


