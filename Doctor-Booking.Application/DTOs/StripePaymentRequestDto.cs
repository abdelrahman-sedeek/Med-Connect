namespace Doctor_Booking.Application.DTOs;

public record StripePaymentRequestDto(
	int BookingId,
	//decimal Amount,
	string PaymentIntentId
);

