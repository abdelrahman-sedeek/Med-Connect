using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Favorites.Commands.AddToFavorites
{
    public class AddToFavoritesCommandHandler : IRequestHandler<AddToFavoritesCommand, ResponseViewModel<object>>
    {
        private readonly IFavoriteRepository repository;

        public AddToFavoritesCommandHandler(IFavoriteRepository repository)
        {
            this.repository = repository;
        }


        public async Task<ResponseViewModel<object>> Handle(AddToFavoritesCommand request, CancellationToken cancellationToken)
        {
            // Check if already in favorites
            var existingFavorite = await repository.GetFavoriteAsync(
                request.PatientId,
                request.DoctorId,
                cancellationToken
            );
            if (existingFavorite != null)
            {
                return ResponseViewModel<object>.FailureResponse(
                    "Doctor is already in patient's favorites.",
                    status: 400
                );
            }

            // check if doctor exists
            var isDoctorExists = await repository.DoctorExistsAsync(request.DoctorId, cancellationToken);
            if (!isDoctorExists)
            {
                return ResponseViewModel<object>.FailureResponse(
                    "Doctor does not exist.",
                    status: 404
                );
            }

            await repository.AddToFavoritesAsync(request.PatientId, request.DoctorId, cancellationToken);
            return ResponseViewModel<object>.SuccessResponse(data: null, status: 201, message: "Doctor added to favorites successfully");
        }
    }
}
