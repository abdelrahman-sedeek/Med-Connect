using Doctor_Booking.Application.Interfaces.Repositories.ChatRepoInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doctor_Booking.Domain.Entities;
using Doctor_Booking.Infastructure.Data;
using MediatR.NotificationPublishers;

namespace Doctor_Booking.Infastructure.Repositories.ChatRepo
{
    public class ChatRepository : IChatRepository
    {
        private readonly DoctorBookingDbContext _context;
        public ChatRepository(DoctorBookingDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Chat chat)
        {
            await _context.Chats.AddAsync(chat);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int chatId)
        {
            return await _context.Chats.AnyAsync(c => c.Id == chatId);
        }

        public async Task<Chat?> GetByIdAsync(int chatId)
        {
            return await _context.Chats.FindAsync(chatId);
        }

        public async Task<Chat?> GetChatWithUsersAsync(int chatId)
        {
            return await _context.Chats
                .Include(c => c.ChatUsers)
                    .ThenInclude(cu => cu.User)
                .FirstOrDefaultAsync(c => c.Id == chatId);
        }

        public async Task<Chat?> GetChatWithMessagesAsync(int chatId)
        {
            return await _context.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == chatId);
        }

        public async Task<List<Chat?>> GetUserChatsAsync(int userId)
        {
            var chats = _context.ChatUsers.Where(c => c.UserId == userId).Select(c=>c.ChatId).ToList();
            var res = await _context.Chats.Where(c => chats.Contains(c.Id)).Include(c => c.ChatUsers).Include(c => c.Messages.OrderBy(c => c.CreatedAt)).ToListAsync();
            return res;
        }
    }
}
