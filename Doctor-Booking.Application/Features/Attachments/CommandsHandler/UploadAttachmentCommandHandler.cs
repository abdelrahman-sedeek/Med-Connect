using Doctor_Booking.Application.Features.Attachments.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.Attachments.CommandsHandler
{
    public class UploadAttachmentCommandHandler : IRequestHandler<UploadAttachmentCommand, string>
    {
        string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png",".mp3" , ".mp4"};
        public Task<string> Handle(UploadAttachmentCommand request, CancellationToken cancellationToken)
        {
            //1. Check the extension
            var extension = Path.GetExtension(request.File.FileName);

            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                return null;
            }
            //2. check the size
            if (request.File.Length > 7 * 1024 * 1024 || request.File.Length == 0)
            {
                return null;
            }
            //3. Get located folder path
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", request.FolderName);
            //4. Get the file name (unique name)
            var fileName = Guid.NewGuid().ToString() + request.File.FileName;
            //5. Get the full path
            var filePath = Path.Combine(folderPath, fileName);
            //6. create file stream
            using FileStream fileStream = new FileStream(filePath, FileMode.Create);
            //7. copy the file to file stream
            request.File.CopyTo(fileStream);
            //8. return the fileName
            return Task.FromResult(fileName);
        }
    }
}
