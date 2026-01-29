using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.SearchHistory.Commands.DeleteSearchHistory
{
    public record DeleteSearchHistoryCommand : IRequest<ResponseViewModel<object>>
    {
        public int Id { get; init; }
    }
}
