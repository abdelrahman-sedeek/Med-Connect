using Doctor_Booking.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.CharCqrs.Commands
{
    public class ChangeChatStatusCommand : IRequest<ResponseViewModel<string>>
    {
        public int chatId { get; set; }
        public int userId { get; set; }
        public string status { get; set; } = string.Empty;
        public ChangeChatStatusCommand(int userId,int chatId,string status)
        {
            this.chatId = chatId;
            this.userId = userId;
            this.status = status;
        }
    }
}
