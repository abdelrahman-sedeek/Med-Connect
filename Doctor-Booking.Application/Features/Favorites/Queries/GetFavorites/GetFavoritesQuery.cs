using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Favorites.Queries.GetFavorites
{
    public record GetFavoritesQuery : IRequest<ResponseViewModel<List<GetFavoritesDto>>>
    {
        public int PatientId { get; init; }
        public GetFavoritesQuery(int patientId)
        {
            PatientId = patientId;
        }
    }
}
