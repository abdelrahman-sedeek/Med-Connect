using Doctor_Booking.Application.Features.Specialties.Commands.CreateSpecialty;
using Doctor_Booking.Application.Features.Specialties.Commands.DeleteSpecialty;
using Doctor_Booking.Application.Features.Specialties.Queries.GetAllSpecialties;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SpecialtiesController : ControllerBase
    {
        private readonly IMediator mediator;
        public SpecialtiesController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllSpecialties()
        {
            var query = new GetAllSpecialtiesQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpecialty([FromBody] CreateSpecialtyCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecialty(int id)
        {
            var command = new DeleteSpecialtyCommand { Id = id };
            var result = await mediator.Send(command);
            return Ok(result);
        }

    }
}
