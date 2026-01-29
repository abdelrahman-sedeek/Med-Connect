using Doctor_Booking.Application.Features.Booking.Cancel.Query;
using Doctor_Booking.Application.Features.Booking.Create.Command;
using Doctor_Booking.Application.Features.Booking.Get.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorBooking.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookingController : ControllerBase
	{
		private IMediator _mediator;

		public BookingController(IMediator mediator)
		{
			_mediator = mediator;
		}


		[Authorize(Roles = "Patient")]
		[HttpPost]
		public async Task<IActionResult> CreateBooking(
		   [FromBody] CreateBookingCommand command)
		{
			if (command == null)
				return BadRequest("Invalid booking request");

			var response = await _mediator.Send(command);

			if (!response.IsSucsess)
				return BadRequest(response);

			return Ok(response);
		}


		[HttpGet]
		public async Task<IActionResult> GetAllBookings(
		[FromQuery] GetAllBookingsQuery query)
		{
			var result = await _mediator.Send(query);
			return Ok(result);
		}


		//GET {{youssefURL}}/api/bookings/by-date?date=2025-12-23&pageNumber=1&pageSize=5
		[HttpGet("date")]
		public async Task<IActionResult> GetBookingsByDate(
	[FromQuery] GetBookingsByDateQuery query)
		{
			var result = await _mediator.Send(query);
			return Ok(result);
		}

		
		//POST {{baseURL}}/api/bookings/cancel
		[HttpPost("cancel")]
		public async Task<IActionResult> CancelBooking(
		[FromBody] CancelBookingCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(result);
		}

	}
}
