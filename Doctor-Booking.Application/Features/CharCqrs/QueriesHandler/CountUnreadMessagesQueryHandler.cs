using Doctor_Booking.Application.Features.CharCqrs.Queries;
using Doctor_Booking.Application.Interfaces.Repositories.ChatRepoInterface;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.CharCqrs.QueriesHandler
{
    public class CountUnreadMessagesQueryHandler : IRequestHandler<CountUnreadMessagesQuery, ResponseViewModel<int>>
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        public CountUnreadMessagesQueryHandler(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }
        public async Task<ResponseViewModel<int>> Handle(CountUnreadMessagesQuery request, CancellationToken cancellationToken)
        {

            var  unreadCount = await _chatMessageRepository.CountUnReadAsync(request.ChatId, request.UserId);
            return ResponseViewModel<int>.SuccessResponse(unreadCount,200,"Request Completed Successfully");
        }
    }
}
