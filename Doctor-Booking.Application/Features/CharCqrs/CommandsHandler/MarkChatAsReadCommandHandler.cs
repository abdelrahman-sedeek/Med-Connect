using Doctor_Booking.Application.Features.CharCqrs.Commands;
using Doctor_Booking.Application.Interfaces.Repositories.ChatRepoInterface;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.CharCqrs.CommandsHandler
{
    public class MarkChatAsReadCommandHandler : IRequestHandler<MarkChatAsReadCommand,ResponseViewModel<string>>
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        public MarkChatAsReadCommandHandler(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }
        public async Task<ResponseViewModel<string>> Handle(MarkChatAsReadCommand request, CancellationToken cancellationToken)
        {
            var messages = await _chatMessageRepository.GetMessagesByChatIdAsync(request.ChatId);
            foreach (var message in messages)
            {
                if (message.SenderUserId != request.UserId && !message.IsRead)
                {
                   await _chatMessageRepository.MarkAsReadAsync(message.Id);
                }
            }
            return ResponseViewModel<string>.SuccessResponse(null, 200, "Marked As Readed");
        }
    }
}
