using FluentValidation;

namespace Doctor_Booking.Application.Features.Favorites.Commands.AddToFavorites
{
    public class AddToFavoritesCommandValidator : AbstractValidator<AddToFavoritesCommand>
    {
        public AddToFavoritesCommandValidator()
        {
            RuleFor(command => command.PatientId).NotEmpty().WithMessage("Patient ID is required.");
            RuleFor(command => command.DoctorId).NotEmpty().WithMessage("Doctor ID is required.");
        }
    }
}
