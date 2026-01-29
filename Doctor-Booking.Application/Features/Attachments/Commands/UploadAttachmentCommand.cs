using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.Attachments.Commands
{
    public class UploadAttachmentCommand : IRequest<string>
    {
        public IFormFile File;
        public string FolderName;
        public UploadAttachmentCommand(IFormFile file, string folderName)
        {
            File = file;
            FolderName = folderName;
        }
    }
}
