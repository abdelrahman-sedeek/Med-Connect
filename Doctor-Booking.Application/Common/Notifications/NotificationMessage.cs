namespace Doctor_Booking.Application.Common.Notifications;

// this is a DTO connect between Handlers and Services
public record NotificationMessage(
	int UserId,
	string Title,
	string Body
);

