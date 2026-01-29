using Doctor_Booking.Application.Common.Notifications;
using Doctor_Booking.Application.Features.Booking.Cancel.Query;
using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Application.Interfaces.Services;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Doctor_Booking.Application.Features.Booking.Cancel.Handler;

public class CancelBookingCommandHandler 
	: IRequestHandler<CancelBookingCommand, ResponseViewModel<bool>>
{
	private readonly IDoctorBookingDbContext _context;
	private readonly INotificationService _notificationService;

	public CancelBookingCommandHandler(IDoctorBookingDbContext context , INotificationService notificationService)
	{
		_context = context;
		_notificationService = notificationService;
	}
	public async Task<ResponseViewModel<bool>> Handle
		(CancelBookingCommand request, CancellationToken cancellationToken)
	{
		var booking = await _context.Bookings
			.Include(b => b.AvailabilitySlot)
			.FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

		if (booking == null)
			return ResponseViewModel<bool>.FailureResponse("Booking not found");

		// 2️- Validate status
		if (booking.Status == BookingStatus.Cancelled)
			return ResponseViewModel<bool>.FailureResponse("Booking already cancelled");

		// 3️- Cancel booking
		booking.Status = BookingStatus.Cancelled;

		// 4️- Free slot
		if (booking.AvailabilitySlot != null)
			booking.AvailabilitySlot.IsBooked = false;

		await _context.SaveChangesAsync(cancellationToken);

		// 🔔 Notify patient
		await _notificationService.CreateAsync(
			new NotificationMessage(
				booking.PatientId!.Value,
				"Booking Cancelled",
				"Your booking has been cancelled"
			),
			cancellationToken
		);

		return ResponseViewModel<bool>
			.SuccessResponse(true, 200 ,"Booking cancelled successfully");
	}
}
