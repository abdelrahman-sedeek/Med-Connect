
﻿using Doctor_Booking.Application.DTOs.Doctor;
﻿using Doctor_Booking.Application.Features.AvailabiltySlot.Query;
using Doctor_Booking.Application.Features.Doctors.Commands;
using Doctor_Booking.Application.Features.Doctors.Queries;
using Doctor_Booking.Application.Features.Doctors.Queries.GetNearbyDoctors;
using Doctor_Booking.Application.Features.Doctors.Queries.SearchDoctors;
using Doctor_Booking.Application.Features.SearchHistory.Commands.SaveSearchHistory;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DoctorBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient,Doctor")]
    public class DoctorsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DoctorsController> _logger;
        public DoctorsController(IMediator mediator, ILogger<DoctorsController> logger) 
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseViewModel<object>.FailureResponse(
                    message: "Validation failed",
                    status: 400,
                    errors: ModelState.Values.SelectMany(v => v.Errors.Select(e => (object)e.ErrorMessage)).ToList()
                ));

            var command = new CreateDoctorCommand(dto);
            var result = await _mediator.Send(command);

            return StatusCode(result.Status, result);
        }
        
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] UpdateDoctorDto dto)
        {
            if (id != dto.Id)
                return BadRequest(ResponseViewModel<object>.FailureResponse(
                    message: "ID mismatch",
                    status: 400
                ));

            if (!ModelState.IsValid)
                return BadRequest(ResponseViewModel<object>.FailureResponse(
                    message: "Validation failed",
                    status: 400,
                    errors: ModelState.Values.SelectMany(v => v.Errors.Select(e => (object)e.ErrorMessage)).ToList()
                ));

            var command = new UpdateDoctorCommand(dto);
            var result = await _mediator.Send(command);

            return StatusCode(result.Status, result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var command = new DeleteDoctorCommand(id);
            var result = await _mediator.Send(command);

            return StatusCode(result.Status, result);
        }

     
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorDetails(int id, ISender sender)
        {
            return Ok(await sender.Send(
                new GetDoctorByIdQuery(id)));
        }



        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearbyDoctors(int? patientId, int? doctorId, double? latitude, double? longitude, double radiusKm = 10, int pageNumber = 1, int pageSize = 10)
        {
            var query = new GetNearbyDoctorsQuery
            {
                PatientId = patientId,
                DoctorId = doctorId,
                Latitude = latitude,
                Longitude = longitude,
                RadiusKm = radiusKm,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchDoctors(int? patientId, int? doctorId, string? query, string? specialty, double? latitude, double? longitude, double radiusKm = 50, int pageNumber = 1, int pageSize = 10)
        {
            var searchQuery = new SearchDoctorsQuery
            {
                Query = query,
                Specialty = specialty,
                Latitude = latitude,
                Longitude = longitude,
                RadiusKm = radiusKm,
                PatientId = patientId,
                DoctorId = doctorId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(searchQuery);

            // Save to history if search returned any results
            if (result != null && result.IsSucsess && result.Data != null && result.Data.Items != null && result.Data.Items.Count > 0)
            {
                try
                {
                    var saveHistoryCommand = new SaveSearchHistoryCommand
                    {
                        PatientId = patientId,
                        DoctorId = doctorId,
                        Query = query,
                        Specialty = specialty,
                        Location = latitude.HasValue && longitude.HasValue ? $"{latitude},{longitude}" : null
                    };

                    // Fire and forget - don't wait for response
                    _ = await _mediator.Send(saveHistoryCommand);
                }
                catch
                {
                    // Ignore save errors - don't fail the search
                    throw new Exception("Adding to search history faild");
                }
            }

            return Ok(result);
        }

       

        //get the slots for the one doctor
        //GET https://localhost:7267/api/doctors/5/slots

        [HttpGet("{doctorId}/slots")]
        public async Task<IActionResult> GetDoctorAvailabilitySlots(int doctorId, ISender sender)
        {
            var result = await sender.Send(
                new GetDoctorAvailabilitySlotsQuery(doctorId));

            return Ok(result);
        }

    }

}
