using Doctor_Booking.Application.Features.CharCqrs.Queries;
using Doctor_Booking.Application.Interfaces.Repositories.ChatRepoInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.CharCqrs.QueriesHandler
{
    public class GetChatUsersQueryHandler : IRequestHandler<GetChatUsersQuery, List<int?>>
    {
        private IChatRepository chatRepository;
        public GetChatUsersQueryHandler(IChatRepository chatRepository)
        {
            this.chatRepository = chatRepository;
        }

        public async Task<List<int?>> Handle(GetChatUsersQuery request, CancellationToken cancellationToken)
        {
            var chat = await chatRepository.GetChatWithUsersAsync(request.chatId);
            var userIds=chat.ChatUsers.Select(x=>x.UserId).ToList();
            return userIds;
        }
    }
}
