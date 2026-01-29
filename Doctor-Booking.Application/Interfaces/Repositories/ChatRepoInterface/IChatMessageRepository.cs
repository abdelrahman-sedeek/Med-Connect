using Doctor_Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Interfaces.Repositories.ChatRepoInterface
{
    public interface IChatMessageRepository
    {
        Task AddAsync(ChatMessage message);
        Task<List<ChatMessage>> GetMessagesByChatIdAsync(int chatId);
        Task MarkAsReadAsync(int messageId);
        Task<int> CountUnReadAsync(int chatId, int userId);
        public Task<int> DeleteMessageAsync(int messageId);
    }
}
