using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Favorites.Commands.RemoveFromFavorites
{
    public class RemoveFromFavoritesCommandHandler : IRequestHandler<RemoveFromFavoritesCommand, ResponseViewModel<object>>
    {
        private readonly IFavoriteRepository favorite;

        public RemoveFromFavoritesCommandHandler(IFavoriteRepository favorite)
        {
            this.favorite = favorite;
        }


        public async Task<ResponseViewModel<object>> Handle(RemoveFromFavoritesCommand request, CancellationToken cancellationToken)
        {
            var isRemoved = await favorite.RemoveFromFavoritesAsync(request.Id, cancellationToken);
            if (!isRemoved)
            {
                return ResponseViewModel<object>.FailureResponse(
                    "Could not remove from favorites.",
                    status: 400
                );
            }
            return ResponseViewModel<object>.SuccessResponse(data: null, message: "Removed from favorites successfully");
        }
    }
}
