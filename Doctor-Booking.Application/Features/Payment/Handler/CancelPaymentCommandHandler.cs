using Doctor_Booking.Application.Common.Notifications;
using Doctor_Booking.Application.Features.Payment.Command;
using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Application.Interfaces.Services;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Entities;
using Doctor_Booking.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Doctor_Booking.Application.Features.Payment.Handler;

public class CancelPaymentCommandHandler
	: IRequestHandler<CancelPaymentCommand, ResponseViewModel<bool>>

{
	private readonly IDoctorBookingDbContext _context;
	private readonly INotificationService _notificationService;
	private readonly IMediator _mediator;

	public CancelPaymentCommandHandler
		(IDoctorBookingDbContext context
		, INotificationService notificationService ,
		IMediator mediator)
	{
		_context = context;
		_notificationService = notificationService;
		_mediator = mediator;
	}
	public async Task<ResponseViewModel<bool>> Handle(CancelPaymentCommand request, CancellationToken cancellationToken)
	{
		// 1️- Get booking
		var booking = await _context.Bookings
			.Include(b => b.Payment)
			.FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

		if (booking == null)
			return ResponseViewModel<bool>.FailureResponse("Booking not found");

		// 2️- Validate status
		if (booking.Status != BookingStatus.Confirmed)
			return ResponseViewModel<bool>
				.FailureResponse("Only confirmed bookings can be cancelled");

		//refund
		if (booking.Payment?.Status == PaymentStatus.Completed)
		{
			// delegate refund to its own command
			return await _mediator.Send(
				new RefundPaymentCommand(request.BookingId),
				cancellationToken);
		}
		//============================

		// 3️- Cancel payment
		if (booking.Payment != null)
		{
			booking.Payment.Status = PaymentStatus.Cancelled;
		}

		// 4️- Cancel booking
		booking.Status = BookingStatus.Cancelled;

		// 5️- Free availability slot
		if (booking.AvailabilitySlotId.HasValue)
		{
			var slot = await _context.AvailabilitySlots
				.FindAsync(booking.AvailabilitySlotId);

			if (slot != null)
				slot.IsBooked = false;
		}

		await _context.SaveChangesAsync(cancellationToken);

		await _notificationService.CreateAsync(
		new NotificationMessage(
		booking.PatientId!.Value,
		"Payment Failed",
		"Your payment could not be processed"
			),
			cancellationToken
		    );

		return ResponseViewModel<bool>
			.SuccessResponse(true, 200 ,"Payment and booking cancelled successfully");
	}
}
