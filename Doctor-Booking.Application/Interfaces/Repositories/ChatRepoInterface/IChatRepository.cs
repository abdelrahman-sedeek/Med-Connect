using Doctor_Booking.Domain.Common;
using Doctor_Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Interfaces.Repositories.ChatRepoInterface
{
    public interface IChatRepository
    {
        Task<Chat?> GetByIdAsync(int chatId);
        Task<Chat?> GetChatWithUsersAsync(int chatId);
        Task<Chat?> GetChatWithMessagesAsync(int chatId);

        Task AddAsync(Chat chat);
        Task<bool> ExistsAsync(int chatId);
        Task<List<Chat?>> GetUserChatsAsync(int userId);
    }
}
