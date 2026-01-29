using Doctor_Booking.Application.DTOs.ChatDtos;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.CharCqrs.Queries
{
    public class GetChatMessagesQuery : IRequest<ResponseViewModel<List<ChatMessageDto>>>
    {
        public int ChatId { get; set; }
        public GetChatMessagesQuery(int chatId)
        {
            ChatId = chatId;
        }
    }
}
