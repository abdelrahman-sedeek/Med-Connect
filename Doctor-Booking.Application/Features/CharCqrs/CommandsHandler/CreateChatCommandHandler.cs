using Doctor_Booking.Application.Features.ChatCqrs.Commands;
using Doctor_Booking.Application.Interfaces.Repositories.ChatRepoInterface;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.ChatCqrs.CommandsHandler
{
    public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, ResponseViewModel<string>>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IChatUserRepository _chatUserRepository;
        public CreateChatCommandHandler(IChatRepository chatRepository, IChatUserRepository chatUserRepository)
        {
            _chatRepository = chatRepository;
            _chatUserRepository = chatUserRepository;
        }
        public async Task<ResponseViewModel<string>> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
           Chat chat = new Chat();
              await _chatRepository.AddAsync(chat);
             foreach (var userId in request.UserIds)
             {
                 await _chatUserRepository.AddUserToChatAsync(chat.Id, userId);
            }
             var userId1 = request.UserIds.First();
            var userId2 = request.UserIds.Last();
            var user1Chats= await _chatRepository.GetUserChatsAsync(userId1);
            foreach(var userChat in user1Chats)
            {
                foreach(var user in userChat.ChatUsers)
                {
                    if(user.UserId == userId2)
                    {
                        return ResponseViewModel<string>.FailureResponse("There Is Already Chat Between Them", 400, "There Is Already Chat Between Them", null);
                    }
                }
            }
            return ResponseViewModel<string>.SuccessResponse($"{chat.Id} is created",200,$"Users [{userId1},{userId2}] is in chat {chat.Id}");
        }
    }
}
