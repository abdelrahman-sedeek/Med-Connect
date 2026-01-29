using Doctor_Booking.Application.DTOs.ChatDtos;
using Doctor_Booking.Application.Features.CharCqrs.Queries;
using Doctor_Booking.Application.Interfaces.Repositories.ChatRepoInterface;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.CharCqrs.QueriesHandler
{
    public class GetUserChatsQueryHandler : IRequestHandler<GetUserChatsQuery, ResponseViewModel<List<ChatDto?>>>
    {
        private readonly IChatRepository _chatRepository;
        public GetUserChatsQueryHandler(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<ResponseViewModel<List<ChatDto?>>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
        {
            var chats = await _chatRepository.GetUserChatsAsync(request.UserId);
            List<ChatDto?> result = new List<ChatDto?>();
            foreach(var chat in chats)
            {
                HashSet<ChatUserDto> userDtos = new HashSet<ChatUserDto>();
                foreach(var user in chat.ChatUsers)
                {
                    userDtos.Add(new ChatUserDto()
                    {
                        ChatId = user.ChatId,
                        UserId = user.UserId,
                        status = user.status
                    });
                }
                HashSet<ChatMessageDto> messageDtos = new HashSet<ChatMessageDto>();
                foreach(var message in chat.Messages)
                {
                    messageDtos.Add(new ChatMessageDto()
                    {    Id=message.Id,
                        ContentType = message.ContentType,
                        MessageCont = message.Message,
                        ChatId = message.ChatId,
                        SenderId = message.SenderUserId
                    });
                }
                result.Add(new ChatDto()
                {
                    ChatUsers = userDtos,
                    Messages = messageDtos
                });
            }
            
            return ResponseViewModel<List<ChatDto?>>.SuccessResponse(result,200,"Request completed successfully");
        }
    }
}
