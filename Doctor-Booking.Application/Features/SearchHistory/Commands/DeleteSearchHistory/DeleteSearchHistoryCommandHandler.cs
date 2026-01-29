using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.SearchHistory.Commands.DeleteSearchHistory
{
    internal class DeleteSearchHistoryCommandHandler : IRequestHandler<DeleteSearchHistoryCommand, ResponseViewModel<object>>
    {
        private readonly ISearchHistoryRepository searchHistoryRepository;
        public DeleteSearchHistoryCommandHandler(ISearchHistoryRepository searchHistoryRepository)
        {
            this.searchHistoryRepository = searchHistoryRepository;
        }


        public async Task<ResponseViewModel<object>> Handle(DeleteSearchHistoryCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await searchHistoryRepository.DeleteSearchHistoryAsync(request.Id, cancellationToken);
            if (!isDeleted)
            {
                return ResponseViewModel<object>.FailureResponse("Deletion failed, please try again later", status: 400);
            }
            return ResponseViewModel<object>.SuccessResponse(data: null, message: "Search history deleted successfully");
        }
    }
}
