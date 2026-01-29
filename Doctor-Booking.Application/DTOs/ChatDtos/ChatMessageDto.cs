using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.DTOs.ChatDtos
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public string? MessageCont { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
        public string ContentType { get; set; } = "Text";
        public bool IsRead { get; set; }
        public int? SenderId { get; set; }
        public int? ChatId   { get; set; }


    }
}
