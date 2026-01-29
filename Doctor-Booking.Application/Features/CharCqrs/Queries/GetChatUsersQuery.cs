using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.CharCqrs.Queries
{
    public class GetChatUsersQuery : IRequest<List<int?>>
    {
        public int chatId { get; set; }
        public GetChatUsersQuery(int chatId)
        {
            this.chatId = chatId;
        }
    }
}
