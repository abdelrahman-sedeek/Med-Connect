using Doctor_Booking.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.ChatCqrs.Commands
{
    public class CreateChatCommand : IRequest<ResponseViewModel<string>>
    {
        public List<int> UserIds { get; set; }
        public CreateChatCommand(List<int> userIds)
        {
            UserIds = userIds;
        }
    }
}
