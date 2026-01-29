using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.SearchHistory.Queries.GetSearchHistory
{
    public class GetSearchHistoryQueryHandler : IRequestHandler<GetSearchHistoryQuery, ResponseViewModel<List<GetSearchHistoryDto>>>
    {
        private readonly ISearchHistoryRepository searchHistoryRepository;
        public GetSearchHistoryQueryHandler(ISearchHistoryRepository searchHistoryRepository)
        {
            this.searchHistoryRepository = searchHistoryRepository;
        }

        public async Task<ResponseViewModel<List<GetSearchHistoryDto>>> Handle(GetSearchHistoryQuery request, CancellationToken cancellationToken)
        {
            var history = await searchHistoryRepository.GetSearchHistoryAsync(request.PatientId, request.DoctorId, request.Limit, cancellationToken);
            return ResponseViewModel<List<GetSearchHistoryDto>>.SuccessResponse(history);
        }
    }
}
