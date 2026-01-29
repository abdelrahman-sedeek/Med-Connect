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
    public class ChatUserRepository : IChatUserRepository
    {

        private readonly DoctorBookingDbContext _context;
        public ChatUserRepository(DoctorBookingDbContext context)
        {
            _context = context;
        }
        public async Task AddUserToChatAsync(int chatId, int userId)
        {
            if (await IsUserInChatAsync(chatId, userId))
                return;

            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = userId
            };

            await _context.ChatUsers.AddAsync(chatUser);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsUserInChatAsync(int chatId, int userId)
        {
            return await _context.ChatUsers
                .AnyAsync(cu => cu.ChatId == chatId && cu.UserId == userId);
        }

        public async Task<List<int?>> GetChatUsersAsync(int chatId)
        {
            return await _context.ChatUsers
                .Where(cu => cu.ChatId == chatId)
                .Select(cu => cu.UserId)
                .ToListAsync();
        }

        public async Task ChangeChatStatus(int userId,int chatId, string status)
        {
           var chatUser=  await _context.ChatUsers.Where(cu => cu.UserId == userId&&cu.ChatId==chatId).FirstOrDefaultAsync();
            chatUser.status=status;
            await _context.SaveChangesAsync();
        }
    }
}
