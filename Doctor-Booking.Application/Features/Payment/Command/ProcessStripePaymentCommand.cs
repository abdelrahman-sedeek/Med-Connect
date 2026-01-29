using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.ViewModels;
using FluentValidation;
using MediatR;

namespace Doctor_Booking.Application.Features.Payment.Command;

public record ProcessStripePaymentCommand(
	StripePaymentRequestDto Payment
) : IRequest<ResponseViewModel<bool>>;

public class ProcessStripePaymentCommandValidator
	: AbstractValidator<ProcessStripePaymentCommand>
{
	public ProcessStripePaymentCommandValidator()
	{
		RuleFor(x => x.Payment.BookingId)
			.GreaterThan(0);

		

		



	}
}
