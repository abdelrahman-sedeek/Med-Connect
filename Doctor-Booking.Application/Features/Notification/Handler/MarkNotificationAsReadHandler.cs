namespace Doctor_Booking.Application.Features.Notification.Handler;

using Doctor_Booking.Application.Features.Notification.Command;
using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class MarkNotificationAsReadHandler
	: IRequestHandler<MarkNotificationAsReadCommand,
		ResponseViewModel<bool>>
{
	private readonly IDoctorBookingDbContext _context;

	public MarkNotificationAsReadHandler(IDoctorBookingDbContext context)
	{
		_context = context;
	}

	public async Task<ResponseViewModel<bool>> Handle(
		MarkNotificationAsReadCommand request,
		CancellationToken cancellationToken)
	{
		var notification = await _context.Notifications
			.FirstOrDefaultAsync(
				n => n.Id == request.NotificationId,
				cancellationToken);

		if (notification == null)
		{
			return ResponseViewModel<bool>
				.FailureResponse("Notification not found");
		}

		if (!notification.IsRead)
		{
			notification.IsRead = true;
			await _context.SaveChangesAsync(cancellationToken);
		}

		return ResponseViewModel<bool>
			.SuccessResponse(true, 200 , "Notification marked as read");
	}
}

