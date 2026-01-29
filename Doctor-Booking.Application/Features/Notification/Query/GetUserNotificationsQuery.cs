using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Notification.Query;

public record GetUserNotificationsQuery(int UserId)
	: IRequest<ResponseViewModel<List<NotificationDto>>>;

