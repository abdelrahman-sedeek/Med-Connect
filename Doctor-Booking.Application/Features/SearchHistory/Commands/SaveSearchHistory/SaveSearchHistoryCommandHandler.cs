using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.SearchHistory.Commands.SaveSearchHistory
{
    public class SaveSearchHistoryCommandHandler : IRequestHandler<SaveSearchHistoryCommand, ResponseViewModel<object>>
    {
        private readonly ISearchHistoryRepository searchHistoryRepository;
        public SaveSearchHistoryCommandHandler(ISearchHistoryRepository searchHistoryRepository)
        {
            this.searchHistoryRepository = searchHistoryRepository;
        }


        public async Task<ResponseViewModel<object>> Handle(SaveSearchHistoryCommand request, CancellationToken cancellationToken)
        {
            await searchHistoryRepository.SaveSearchHistoryAsync(request.PatientId, request.DoctorId, request.Query, request.Specialty, request.Location, cancellationToken);
            return ResponseViewModel<object>.SuccessResponse(data: null, message: "Search history saved successfully");
        }
    }
}
