using Doctor_Booking.Application.Features.Payment.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorBooking.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private IMediator _mediator;

		public PaymentController(IMediator mediator)
		{
			_mediator = mediator;
		}

		//"tok_visa"
		[HttpPost("stripe")]
		public async Task<IActionResult> PayWithStripe(
			[FromBody] ProcessStripePaymentCommand command)
		{
			var response = await _mediator.Send(command);

			if (!response.IsSucsess)
				return BadRequest(response);

			return Ok(response);
		}

		[HttpPost("cancel")]
		public async Task<IActionResult> CancelPayment(
		[FromBody] CancelPaymentCommand command)
		{
			var result = await _mediator.Send(command);

			if (!result.IsSucsess)
				return BadRequest(result);

			return Ok(result);
		}
	}
}
