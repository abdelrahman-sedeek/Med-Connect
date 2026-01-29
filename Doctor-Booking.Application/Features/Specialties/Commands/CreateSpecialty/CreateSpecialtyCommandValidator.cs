using FluentValidation;

namespace Doctor_Booking.Application.Features.Specialties.Commands.CreateSpecialty
{
    public class CreateSpecialtyCommandValidator : AbstractValidator<CreateSpecialtyCommand>
    {
        public CreateSpecialtyCommandValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Specialty name is required.")
            .MaximumLength(100)
            .WithMessage("Specialty name cannot exceed 100 characters.")
            .Matches(@"^[a-zA-Z\s]+$")
            .WithMessage("Specialty name can only contain letters and spaces.");
        }
    }
}
