using Doctor_Booking.Application.Features.SearchHistory.Commands.DeleteSearchHistory;
using Doctor_Booking.Application.Features.SearchHistory.Queries.GetSearchHistory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient,Doctor")]
    public class SearchHistoryController : ControllerBase
    {
        private readonly IMediator mediator;

        public SearchHistoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        //[HttpPost]
        //public async Task<IActionResult> SaveSearchHistory()
        //{
        //    var command = new SaveSearchHistoryCommand();
        //    var result = await mediator.Send(command);
        //    return Created(string.Empty, result);
        //}

        [HttpGet]
        public async Task<IActionResult> GetSearchHistory(int patientId, int limit = 10)
        {
            var query = new GetSearchHistoryQuery
            {
                PatientId = patientId,
                Limit = limit
            };

            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSearchHistory(int id)
        {
            var command = new DeleteSearchHistoryCommand { Id = id };
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}
