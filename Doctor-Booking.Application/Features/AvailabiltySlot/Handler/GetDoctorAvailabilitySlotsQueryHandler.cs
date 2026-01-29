using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.Features.AvailabiltySlot.Query;
using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Doctor_Booking.Application.Features.AvailabiltySlot.Handler;

public class GetDoctorAvailabilitySlotsQueryHandler
	: IRequestHandler<
		GetDoctorAvailabilitySlotsQuery,
		ResponseViewModel<List<AvailabilitySlotDto>>>
{
	private readonly IDoctorBookingDbContext _context;

	public GetDoctorAvailabilitySlotsQueryHandler(
		IDoctorBookingDbContext context)
	{
		_context = context;
	}

	public async Task<ResponseViewModel<List<AvailabilitySlotDto>>> Handle(
		GetDoctorAvailabilitySlotsQuery request,
		CancellationToken cancellationToken)
	{
		var slots = await _context.AvailabilitySlots
			.Where(s => s.DoctorId == request.DoctorId)
			.OrderBy(s => s.StartTime)
			.Select(s => new AvailabilitySlotDto(
				s.Id,
				s.StartTime,
				s.EndTime,
				s.IsBooked
			))
			.ToListAsync(cancellationToken);

		return ResponseViewModel<List<AvailabilitySlotDto>>
			.SuccessResponse(slots);
	}
}
