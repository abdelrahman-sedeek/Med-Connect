using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.SearchHistory.Queries.GetSearchHistory
{
    public record GetSearchHistoryQuery : IRequest<ResponseViewModel<List<GetSearchHistoryDto>>>
    {
        public int? PatientId { get; init; }
        public int? DoctorId { get; init; }
        public int Limit { get; init; } = 10;
    }
}
