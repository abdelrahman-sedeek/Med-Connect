using Doctor_Booking.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.CharCqrs.Commands
{
    public class MarkChatAsReadCommand : IRequest<ResponseViewModel<string>>
    {
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public MarkChatAsReadCommand(int chatId, int userId)
        {
            ChatId = chatId;
            UserId = userId;
        }
    }
}
