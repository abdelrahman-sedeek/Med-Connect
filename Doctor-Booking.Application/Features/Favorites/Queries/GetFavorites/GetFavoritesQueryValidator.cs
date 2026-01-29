using FluentValidation;

namespace Doctor_Booking.Application.Features.Favorites.Queries.GetFavorites
{
    public class GetFavoritesQueryValidator : AbstractValidator<GetFavoritesQuery>
    {
        public GetFavoritesQueryValidator()
        {
            RuleFor(x => x.PatientId).NotNull().NotEmpty().GreaterThan(0).WithMessage("patientId is required");
        }
    }
}
