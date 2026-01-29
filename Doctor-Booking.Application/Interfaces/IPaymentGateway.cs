namespace Doctor_Booking.Application.Interfaces;

public interface IPaymentGateway
{
	Task<string> CreatePaymentAsync(
		  decimal amount,
		  string currency,
		  string description,
		  string token);
}
