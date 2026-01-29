using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Favorites.Queries.GetFavorites
{
    public class GetFavoritesQueryHandler : IRequestHandler<GetFavoritesQuery, ResponseViewModel<List<GetFavoritesDto>>>
    {
        private readonly IFavoriteRepository repository;

        public GetFavoritesQueryHandler(IFavoriteRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ResponseViewModel<List<GetFavoritesDto>>> Handle(GetFavoritesQuery request, CancellationToken cancellationToken)
        {
            var isPatientExists = await repository.PatientExistsAsync(request.PatientId, cancellationToken);
            if (!isPatientExists)
            {
                return ResponseViewModel<List<GetFavoritesDto>>.FailureResponse(
                    $"Patient with ID {request.PatientId} not found.",
                    status: 404
                );
            }
            var favorites = await repository.GetFavoritesAsync(request.PatientId, cancellationToken);
            return ResponseViewModel<List<GetFavoritesDto>>.SuccessResponse(favorites);
        }
    }
}
