using Microsoft.AspNetCore.SignalR;

namespace DoctorBooking.Hubs;


public class NotificationHub : Hub
{
	public override async Task OnConnectedAsync()
	{
		// هنا هنربط كل Connection بـ UserId
		var userId = Context.UserIdentifier;

		if (!string.IsNullOrEmpty(userId))
		{
			await Groups.AddToGroupAsync(
				Context.ConnectionId,
				userId
			);
		}

		await base.OnConnectedAsync();
	}
}
