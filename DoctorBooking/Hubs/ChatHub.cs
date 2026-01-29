using Microsoft.AspNetCore.SignalR;

namespace DoctorBooking.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinChat(int chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

    }
}
