using Doctor_Booking.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.CharCqrs.Queries
{
    public class CountUnreadMessagesQuery : IRequest<ResponseViewModel<int>>
    {
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public CountUnreadMessagesQuery(int userId, int chatId)
        {
            UserId = userId;
            ChatId = chatId;
        }
    }
}
