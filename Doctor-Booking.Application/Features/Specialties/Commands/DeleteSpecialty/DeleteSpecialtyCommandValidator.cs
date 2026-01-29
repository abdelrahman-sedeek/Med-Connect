using FluentValidation;

namespace Doctor_Booking.Application.Features.Specialties.Commands.DeleteSpecialty
{
    public class DeleteSpecialtyCommandValidator : AbstractValidator<DeleteSpecialtyCommand>
    {
        public DeleteSpecialtyCommandValidator()
        {
            RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
        }
    }
}
