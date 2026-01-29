using Doctor_Booking.Application.Features.Attachments.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.Attachments.CommandsHandler
{
    public class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand, bool>
    {
        public Task<bool> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
        {
            if (File.Exists(request.FilePath))
            {
                File.Delete(request.FilePath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
