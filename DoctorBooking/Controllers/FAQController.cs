using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.Features.FAQS.Commands;
using Doctor_Booking.Application.Features.FAQS.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorBooking.Controllers
{
    [ApiController]
    [Route("api/FAQ")]
    
    public class FAQController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FAQController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddFaq(FAQDto faq)
        {
            var res = await _mediator.Send(new AddFAQCommand(faq));
            if (res.IsSucsess) return Ok(res);
            else return BadRequest(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllFaqs()
        {
            var res =await _mediator.Send(new GetAllFAQSQuery());
            if (res.IsSucsess) return Ok(res);
            else return BadRequest(res);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFaq(int id)
        {
            var res = await _mediator.Send(new GetFAQQuery(id));
            if (res.IsSucsess) return Ok(res);
            else return BadRequest(res);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFaq(int id)
        {
            var res = await _mediator.Send(new DeleteFAQCommand(id));
            if (res.IsSucsess) return Ok(res);
            else return BadRequest(res);
        }
        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateFaq(FAQDto faq)
        {
            var res = await _mediator.Send(new  UpdateFAQCommand(faq));
            if (res.IsSucsess) return Ok(res);
            else return BadRequest(res);
        } 
    }
}
