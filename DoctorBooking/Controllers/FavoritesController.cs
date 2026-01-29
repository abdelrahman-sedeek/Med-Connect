using Doctor_Booking.Application.Features.Favorites.Commands.AddToFavorites;
using Doctor_Booking.Application.Features.Favorites.Commands.RemoveFromFavorites;
using Doctor_Booking.Application.Features.Favorites.Queries.GetFavorites;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class FavoritesController : ControllerBase
    {
        private readonly IMediator mediator;

        public FavoritesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetFavorites([FromRoute] int patientId)
        {
            var query = new GetFavoritesQuery(patientId);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites([FromQuery] int patientId, [FromQuery] int doctorId)
        {
            var result = await mediator.Send(new AddToFavoritesCommand(patientId, doctorId));
            return Created(string.Empty, result);
        }

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> RemoveFromFavorites([FromRoute] int reviewId)
        {
            var result = await mediator.Send(new RemoveFromFavoritesCommand(reviewId));
            return Ok(result);
        }
    }
}
