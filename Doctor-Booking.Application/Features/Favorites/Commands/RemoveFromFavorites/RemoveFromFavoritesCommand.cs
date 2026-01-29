using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Favorites.Commands.RemoveFromFavorites
{
    public record RemoveFromFavoritesCommand : IRequest<ResponseViewModel<object>>
    {
        public int Id { get; init; }
        public RemoveFromFavoritesCommand(int reviewId)
        {
            Id = reviewId;
        }
    }
}
