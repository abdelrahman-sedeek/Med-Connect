using Doctor_Booking.Application.Features.ToggleNotification.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorBooking.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProfileController : ControllerBase
	{
		private IMediator _mediator;

		public ProfileController(IMediator mediator)
		{
			_mediator = mediator;
		}

		// 🔔 Toggle Notifications
		//POST {{basefURL}}/api/profile/notifications/toggle
		[HttpPost("notifications/toggle")]
		public async Task<IActionResult> ToggleNotifications(
			[FromBody] ToggleNotificationsCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(result);
		}
	}
}
