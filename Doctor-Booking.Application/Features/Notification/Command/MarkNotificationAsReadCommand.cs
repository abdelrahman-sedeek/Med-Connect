using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Notification.Command;

public record MarkNotificationAsReadCommand(int NotificationId)
	: IRequest<ResponseViewModel<bool>>;

