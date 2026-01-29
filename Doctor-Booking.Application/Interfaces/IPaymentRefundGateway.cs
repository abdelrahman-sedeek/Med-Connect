
using Doctor_Booking.Application.Common.Payment;

namespace Doctor_Booking.Application.Interfaces;

public interface IPaymentRefundGateway
{
	Task<RefundResult> RefundAsync(
		string transactionRef,
		decimal amount,
		CancellationToken cancellationToken);
}

