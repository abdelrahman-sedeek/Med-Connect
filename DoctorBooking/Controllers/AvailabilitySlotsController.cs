using Doctor_Booking.Application.Features.AvailabilitySlots.Commands.CreateAvailabilitySlot;
using Doctor_Booking.Application.Features.AvailabilitySlots.Commands.DeleteAvailabilitySlot;
using Doctor_Booking.Application.Features.AvailabilitySlots.Commands.UpdateAvailabilitySlot;
using Doctor_Booking.Application.Features.AvailabilitySlots.Queries.GetAllAvailabilitySlots;
using Doctor_Booking.Application.Features.AvailabilitySlots.Queries.GetAvailabilitySlotById;
using Doctor_Booking.Application.Features.AvailabilitySlots.Queries.GetDoctorAvailabilitySlots;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor")]
    public class AvailabilitySlotsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AvailabilitySlotsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get all availability slots
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAvailabilitySlots()
        {
            var query = new GetAllAvailabilitySlotsQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get availability slot by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAvailabilitySlotById(int id)
        {
            var query = new GetAvailabilitySlotByIdQuery { Id = id };
            var result = await mediator.Send(query);
            return result.IsSucsess ? Ok(result) : StatusCode(result.Status, result);
        }

        /// <summary>
        /// Get all availability slots for a specific doctor
        /// </summary>
        [HttpGet("doctor/{doctorId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDoctorAvailabilitySlots(int doctorId)
        {
            var query = new GetDoctorAvailabilitySlotsQuery { DoctorId = doctorId };
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Create a new availability slot
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAvailabilitySlot([FromBody] CreateAvailabilitySlotCommand command)
        {
            var result = await mediator.Send(command);
            return result.IsSucsess ? Created(string.Empty, result) : StatusCode(result.Status, result);
        }

        /// <summary>
        /// Update an availability slot
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAvailabilitySlot(int id, [FromBody] UpdateAvailabilitySlotCommand command)
        {
            if (id != command.Id)
            {
                var errorResponse = ResponseViewModel<bool>.FailureResponse(
                    message: "ID in URL does not match ID in request body.",
                    status: 400
                );
                return BadRequest(errorResponse);
            }

            var result = await mediator.Send(command);
            return result.IsSucsess ? Ok(result) : StatusCode(result.Status, result);
        }

        /// <summary>
        /// Delete an availability slot
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvailabilitySlot(int id)
        {
            var command = new DeleteAvailabilitySlotCommand { Id = id };
            var result = await mediator.Send(command);
            return result.IsSucsess ? Ok(result) : StatusCode(result.Status, result);
        }
    }
}
