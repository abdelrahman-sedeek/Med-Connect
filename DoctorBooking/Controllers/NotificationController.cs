using Doctor_Booking.Application.Features.Notification.Command;
using Doctor_Booking.Application.Features.Notification.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorBooking.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NotificationController : ControllerBase
	{
		private IMediator _mediator;

		public NotificationController( IMediator mediator)
		{
			_mediator = mediator;
		}


		//GET {{baseUrl}}/api/notifications?userId=5
		[HttpGet]
		public async Task<IActionResult> GetUserNotifications(
		[FromQuery] int userId)
		{
			var result = await _mediator.Send(
				new GetUserNotificationsQuery(userId));

			return Ok(result);
		}

		// ✅ Mark notification as read
		//POST {{baseUrl}}/api/notifications/read
		[HttpPost("read")]
		public async Task<IActionResult> MarkAsRead(
			[FromBody] MarkNotificationAsReadCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(result);
		}
	}
}
