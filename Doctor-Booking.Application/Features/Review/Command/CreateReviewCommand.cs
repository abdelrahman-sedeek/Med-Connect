using Doctor_Booking.Application.ViewModels;
using FluentValidation;
using MediatR;

namespace Doctor_Booking.Application.Features.Review.Command;

public record CreateReviewCommand(
	int BookingId,
	int Rating,
	string Comment
) : IRequest<ResponseViewModel<bool>>;

public class CreateReviewCommandValidator
	: AbstractValidator<CreateReviewCommand>
{
	public CreateReviewCommandValidator()
	{
		RuleFor(x => x.BookingId)
			.GreaterThan(0)
			.WithMessage("BookingId must be valid");

		RuleFor(x => x.Rating)
			.InclusiveBetween(1, 5)
			.WithMessage("Rating must be between 1 and 5");

		RuleFor(x => x.Comment)
			.NotEmpty()
			.MaximumLength(500);
	}
}