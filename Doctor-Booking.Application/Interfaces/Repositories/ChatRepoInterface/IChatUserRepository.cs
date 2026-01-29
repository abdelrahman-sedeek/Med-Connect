using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Interfaces.Repositories.ChatRepoInterface
{
    public interface IChatUserRepository
    {
        Task AddUserToChatAsync(int chatId, int userId);
        Task<bool> IsUserInChatAsync(int chatId, int userId);
        Task<List<int?>> GetChatUsersAsync(int chatId);
        Task ChangeChatStatus(int userId,int chatId,string status);
    }
}
