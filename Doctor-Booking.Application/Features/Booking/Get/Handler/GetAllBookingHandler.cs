using Doctor_Booking.Application.Common.Models;
using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.Features.Booking.Get.Query;
using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Doctor_Booking.Application.Features.Booking.Get.Handler;

public class GetAllBookingHandler : IRequestHandler<GetAllBookingsQuery, ResponseViewModel<PaginatedList<BookingListDto>>>

{
	private IDoctorBookingDbContext _context;

	public GetAllBookingHandler(IDoctorBookingDbContext context)
	{
		_context = context;
	}
	public async Task<ResponseViewModel<PaginatedList<BookingListDto>>> Handle
		(GetAllBookingsQuery request, CancellationToken cancellationToken)
	{
		var query = _context.Bookings
		.Include(b => b.Doctor).ThenInclude(d => d.User)
		.Include(b => b.Patient).ThenInclude(p => p.User)
		.Include(b => b.AvailabilitySlot)
		.AsNoTracking()
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
