using Doctor_Booking.Application.DTOs.ChatDtos;
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
    public class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, ResponseViewModel<List<ChatMessageDto>>>
    {
        private readonly IChatRepository _chatRepository;
        public GetChatMessagesQueryHandler(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }
        public async Task<ResponseViewModel<List<ChatMessageDto>>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
        {
            var chat =await _chatRepository.GetChatWithMessagesAsync(request.ChatId);
            var messagesDto = chat.Messages.Select(m => new ChatMessageDto
            {
                Id= m.Id,
                MessageCont = m.Message,
                ContentType = m.ContentType,
                IsRead = m.IsRead,
                SenderId = m.SenderUserId,
                ChatId=m.ChatId,
            }).ToList();
            return ResponseViewModel<List<ChatMessageDto>>.SuccessResponse(messagesDto,200,"Request Completed successfully");
        }
    }
}
