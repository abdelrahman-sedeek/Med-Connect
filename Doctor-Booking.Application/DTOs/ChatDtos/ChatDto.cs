using Doctor_Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.DTOs.ChatDtos
{
    public class ChatDto
    {
        public ICollection<ChatUserDto> ChatUsers { get; set; } = new HashSet<ChatUserDto>();
        public ICollection<ChatMessageDto> Messages { get; set; } = new HashSet<ChatMessageDto>();
    }
}
