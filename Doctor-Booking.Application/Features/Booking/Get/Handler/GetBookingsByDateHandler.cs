using Doctor_Booking.Application.Common.Models;
using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.Features.Booking.Get.Query;
using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Doctor_Booking.Application.Features.Booking.Get.Handler;

public class GetBookingsByDateHandler
	: IRequestHandler<GetBookingsByDateQuery, ResponseViewModel<PaginatedList<BookingListDto>>>
{
	private readonly IDoctorBookingDbContext _context;

	public GetBookingsByDateHandler(IDoctorBookingDbContext context)
	{
		_context = context;
	}
	public async Task<ResponseViewModel<PaginatedList<BookingListDto>>> Handle(GetBookingsByDateQuery request, CancellationToken cancellationToken)
	{
		var query = _context.Bookings
			.Include(b => b.Doctor).ThenInclude(d => d.User)
			.Include(b => b.Patient).ThenInclude(p => p.User)
			.Include(b => b.AvailabilitySlot)
			.AsNoTracking()
			.Where(b =>
				b.AvailabilitySlot != null &&
				DateOnly.FromDateTime(b.AvailabilitySlot.StartTime) == request.Date
			)
			.Select(b => new BookingListDto
			{
				BookingId = b.Id,
				DocName = b.Doctor.User.FirstName + " " + b.Doctor.User.LastName,
				PatientName = b.Patient.User.FirstName + " " + b.Patient.User.LastName,
				Appointment = b.AvailabilitySlot.StartTime,
				status = b.Status
			});

		var result = PaginatedList<BookingListDto>.Create(
			query,
			request.PageNumber,
			request.PageSize
		);

		return ResponseViewModel<PaginatedList<BookingListDto>>
			.SuccessResponse(result);
	}
}
