using Doctor_Booking.Application.Features.CharCqrs.Commands;
using Doctor_Booking.Application.Interfaces.Repositories.ChatRepoInterface;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.CharCqrs.CommandsHandler
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand,ResponseViewModel<string>>
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        public SendMessageCommandHandler(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }
        public async Task<ResponseViewModel<string>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var chatId = request.ChatId;
            ChatMessage message= new ChatMessage()
            {
                ChatId = request.ChatId,
                SenderUserId = request.SenderId,
                Message = request.Content,
                ContentType = request.Type,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await _chatMessageRepository.AddAsync(message);
            return ResponseViewModel<string>.SuccessResponse();
        }
    }
}
