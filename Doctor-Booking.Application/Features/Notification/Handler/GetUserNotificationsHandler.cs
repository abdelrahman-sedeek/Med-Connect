using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.Features.Notification.Query;
using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Doctor_Booking.Application.Features.Notification.Handler;

public class GetUserNotificationsHandler 
	: IRequestHandler<GetUserNotificationsQuery, ResponseViewModel<List<NotificationDto>>>
{
	private IDoctorBookingDbContext _context;

	public GetUserNotificationsHandler(IDoctorBookingDbContext context)
	{
		_context = context;
	}
	public async Task<ResponseViewModel<List<NotificationDto>>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
	{
		var notifications = await _context.Notifications
		.Where(n => n.UserId == request.UserId)
		.OrderByDescending(n => n.CreatedAt)
		.Select(n => new NotificationDto(
		n.Id,
		n.Title,
		n.Message,
		n.IsRead,
		n.CreatedAt
	))
	.ToListAsync();

		return ResponseViewModel<List<NotificationDto>>
			.SuccessResponse(notifications);

	}
}
