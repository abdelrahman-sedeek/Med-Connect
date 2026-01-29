using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.Attachments.Commands
{
    public class DeleteAttachmentCommand : IRequest<bool>
    {
        public string FilePath { get; set; }
        public DeleteAttachmentCommand(string filePath)
        {
            FilePath=filePath;
        }
    }
}
