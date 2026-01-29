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
    public class ChangeChatStatusCommandHandler : IRequestHandler<ChangeChatStatusCommand,ResponseViewModel<string>>
    {
        private readonly IChatUserRepository _chatUserRepository;
        public ChangeChatStatusCommandHandler(IChatUserRepository chatUserRepository)
        {
            _chatUserRepository = chatUserRepository;
        }

        public async Task<ResponseViewModel<string>> Handle(ChangeChatStatusCommand request, CancellationToken cancellationToken)
        {
            await _chatUserRepository.ChangeChatStatus(request.userId, request.chatId, request.status);
            ResponseViewModel<string> response= new ResponseViewModel<string>();
            return ResponseViewModel<string>.SuccessResponse(null,200,$"Status Changed To {request.status}");
        }
    }
}
