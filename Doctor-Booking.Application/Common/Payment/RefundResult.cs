namespace Doctor_Booking.Application.Common.Payment;

public class RefundResult
{
	public bool Success { get; init; }
	public string? RefundReference { get; init; }
	public string? ErrorMessage { get; init; }

	public static RefundResult Ok(string refundReference)
		=> new()
		{
			Success = true,
			RefundReference = refundReference
		};

	public static RefundResult Fail(string error)
		=> new()
		{
			Success = false,
			ErrorMessage = error
		};
}
