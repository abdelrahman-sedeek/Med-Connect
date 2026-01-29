using Doctor_Booking.Application.Interfaces.Repositories.ChatRepoInterface;
using Doctor_Booking.Domain.Entities;
using Doctor_Booking.Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Infastructure.Repositories.ChatRepo
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly DoctorBookingDbContext _context;
        public ChatMessageRepository(DoctorBookingDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(ChatMessage message)
        {
            await _context.ChatMessages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountUnReadAsync(int chatId, int userId)
        {
            return await _context.ChatMessages
                .Where(m =>
                    m.ChatId == chatId &&
                    m.SenderUserId != userId &&
                    !m.IsRead
                )
                .CountAsync();
        }

        public async Task<int> DeleteMessageAsync(int messageId)
        {
            var message=_context.ChatMessages.Where(m=>m.Id==messageId).First();
            _context.ChatMessages.Remove(message);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ChatMessage>> GetMessagesByChatIdAsync(int chatId)
        {
            return await _context.ChatMessages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int messageId)
        {
            var message = await _context.ChatMessages.FindAsync(messageId);
            if (message == null) return;

            message.IsRead = true;
            await _context.SaveChangesAsync();
        }
        
    }
}
