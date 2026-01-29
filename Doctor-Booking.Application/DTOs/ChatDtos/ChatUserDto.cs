using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.DTOs.ChatDtos
{
    public class ChatUserDto
    {
        public int? ChatId { get; set; }
        public int? UserId { get; set; }
        public string status { get; set; } = "normal";
    }
}
