
using Doctor_Booking.Application.Common.Notifications;
using Doctor_Booking.Application.Features.Payment.Command;
using Doctor_Booking.Application.Interfaces.Services;
using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Doctor_Booking.Application.Features.Payment.Handler;

public class RefundPaymentHandler
: IRequestHandler<RefundPaymentCommand, ResponseViewModel<bool>>
{
	private readonly IDoctorBookingDbContext _context;
	private readonly IPaymentRefundGateway _refundGateway;
	private readonly INotificationService _notificationService;

	public RefundPaymentHandler(
		IDoctorBookingDbContext context,
		IPaymentRefundGateway refundGateway,
		INotificationService notificationService)
	{
		_context = context;
		_refundGateway = refundGateway;
		_notificationService = notificationService;
	}

	public async Task<ResponseViewModel<bool>> Handle(
		RefundPaymentCommand request,
		CancellationToken cancellationToken)
	{
		// 1️⃣ Get booking with payment
		var booking = await _context.Bookings
			.Include(b => b.Payment)
			.FirstOrDefaultAsync(
				b => b.Id == request.BookingId,
				cancellationToken);

		if (booking == null)
			return ResponseViewModel<bool>
				.FailureResponse("Booking not found");

		if (booking.Payment == null)
			return ResponseViewModel<bool>
				.FailureResponse("Payment not found");

		// 2️⃣ Validate payment status
		if (booking.Payment.Status != PaymentStatus.Completed)
			return ResponseViewModel<bool>
				.FailureResponse("Only completed payments can be refunded");

		if (booking.Payment.Status == PaymentStatus.Refunded)
			return ResponseViewModel<bool>
				.FailureResponse("Payment already refunded");

		if (booking.Status != BookingStatus.Confirmed)
			return ResponseViewModel<bool>
				.FailureResponse("Only confirmed bookings can be refunded");

		if (string.IsNullOrWhiteSpace(booking.Payment.TransactionRef))
			return ResponseViewModel<bool>
				.FailureResponse("Invalid payment transaction reference");

		// 3️⃣ Call payment gateway (Stripe)
		var refundResult = await _refundGateway.RefundAsync(
			booking.Payment.TransactionRef,
			booking.Payment.Amount,
			cancellationToken);

		if (!refundResult.Success)
			return ResponseViewModel<bool>
				.FailureResponse(refundResult.ErrorMessage ?? "Refund failed");

		// 4️⃣ Update domain state
		booking.Payment.Status = PaymentStatus.Refunded;
		booking.Status = BookingStatus.Cancelled;

		// 5️⃣ Free availability slot (optional but recommended)
		if (booking.AvailabilitySlotId.HasValue)
		{
			var slot = await _context.AvailabilitySlots
				.FindAsync(booking.AvailabilitySlotId);

			if (slot != null)
				slot.IsBooked = false;
		}

		await _context.SaveChangesAsync(cancellationToken);

		// 6️⃣ Send notification
		await _notificationService.CreateAsync(
			new NotificationMessage(
				booking.PatientId!.Value,
				"Payment Refunded",
				"Your payment has been refunded successfully"
			),
			cancellationToken
		);
		await _notificationService.CreateAsync(
	new NotificationMessage(
		booking.DoctorId!.Value,
		"Booking Cancelled",
		"A booking was cancelled and refunded"
	),
	cancellationToken
);

		return ResponseViewModel<bool>
			.SuccessResponse(true, 200, "Payment refunded successfully");
	}
}
