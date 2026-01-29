using FluentValidation;

namespace Doctor_Booking.Application.Features.Favorites.Commands.RemoveFromFavorites
{
    public class RemoveFromFavoritesCommandValidator : AbstractValidator<RemoveFromFavoritesCommand>
    {
        public RemoveFromFavoritesCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("reviewId is required.");
        }
    }
}
