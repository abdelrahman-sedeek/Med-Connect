using Doctor_Booking.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.CharCqrs.Commands
{
    public class DeleteMessageCommand : IRequest<ResponseViewModel<string>>
    {
        public int MessageId { get; set; }
        public DeleteMessageCommand(int messageId)
        {
            MessageId= messageId;
        }
    }
}
