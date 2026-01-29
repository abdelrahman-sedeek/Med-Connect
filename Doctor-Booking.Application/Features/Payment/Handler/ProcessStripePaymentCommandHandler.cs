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

public class ProcessStripePaymentCommandHandler
	: IRequestHandler<ProcessStripePaymentCommand, ResponseViewModel<bool>>
{
	private readonly IDoctorBookingDbContext _context;
	private readonly IPaymentGateway _paymentGateway;
	private readonly INotificationService _notificationService;

	public ProcessStripePaymentCommandHandler(
		IDoctorBookingDbContext context,
		IPaymentGateway paymentGateway,
		INotificationService notificationService)
	{
		_context = context;
		_paymentGateway = paymentGateway;
		_notificationService = notificationService;
	}

	public async Task<ResponseViewModel<bool>> Handle(
		ProcessStripePaymentCommand request,
		CancellationToken cancellationToken)
	{
		var dto = request.Payment;

		// 1️⃣ Load booking + relations
		var booking = await _context.Bookings
			.Include(b => b.Patient)
			.Include(b => b.Payment)
			.Include(b=> b.Doctor)
			.FirstOrDefaultAsync(
				b => b.Id == dto.BookingId,
				cancellationToken);

		if (booking == null)
			return ResponseViewModel<bool>.FailureResponse("Booking not found");


		if (booking.Payment != null &&
			booking.Payment.Status == PaymentStatus.Completed)
		{
			return ResponseViewModel<bool>
				.FailureResponse("Booking already has a completed payment");
		}


		// 2️⃣ Get amount from backend (SECURE)
		var amount = booking.Doctor.SessionPrice; 

		// 3️⃣ Call Stripe (PaymentIntent)
		var transactionRef = await _paymentGateway.CreatePaymentAsync(
			amount,
			"usd",
			$"Booking #{booking.Id}",
			dto.PaymentIntentId // string pm_xxx
		);

		// 4️⃣ Save payment
		var payment = new Domain.Entities.Payment
		{
			Amount = amount,
			BookingId = booking.Id,
			TransactionRef = transactionRef, // pi_xxx
			Status = PaymentStatus.Completed,
			PaymentMethodId = null
		};

		_context.Payments.Add(payment);

		// 5️⃣ Update booking status
		booking.Status = BookingStatus.Confirmed;

		await _context.SaveChangesAsync(cancellationToken);

		// 🔔 Notify patient
		await _notificationService.CreateAsync(
			new NotificationMessage(
				booking.PatientId!.Value,
				"Payment Successful",
				$"Your payment of {amount} was successful"
			),
			cancellationToken
		);

		// 🔔 Notify doctor
		await _notificationService.CreateAsync(
			new NotificationMessage(
				booking.DoctorId!.Value,
				"Booking Confirmed",
				"A booking has been confirmed after payment"
			),
			cancellationToken
		);

		return ResponseViewModel<bool>
			.SuccessResponse(true, 200, "Payment completed successfully");
	}
}

