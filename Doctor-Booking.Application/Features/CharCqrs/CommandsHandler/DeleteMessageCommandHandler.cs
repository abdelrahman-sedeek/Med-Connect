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
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, ResponseViewModel<string>>
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        public DeleteMessageCommandHandler(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }
        public async Task<ResponseViewModel<string>> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
              var res = await _chatMessageRepository.DeleteMessageAsync(request.MessageId);
            if (res == 0)
            {
                return ResponseViewModel<string>.FailureResponse("This message is already deleted or not found", 400, "This message is already deleted or not found", null);
            }
            else return ResponseViewModel<string>.SuccessResponse($"Message with id {request.MessageId} has deleted successfully", 200, "Request completed successfully");
        }
    }
}
