using Doctor_Booking.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.CharCqrs.Commands
{
    public class SendMessageCommand : IRequest<ResponseViewModel<string>>
    {
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public SendMessageCommand(int chatId, int senderId, string content, string type)
        {
            ChatId = chatId;
            SenderId = senderId;
            Content = content;
            Type = type;
        }
    }
}
