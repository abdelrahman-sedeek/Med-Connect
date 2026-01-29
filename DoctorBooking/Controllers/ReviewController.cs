using Doctor_Booking.Application.Features.Review.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorBooking.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReviewController : ControllerBase
	{
		private IMediator _mediator;

		public ReviewController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[Authorize(Roles = "Patient")]
		[HttpPost("reviews")]
		public async Task<IActionResult> CreateReview(
	CreateReviewCommand command)
		{
			return Ok(await _mediator.Send(command));
		}

	}
}
