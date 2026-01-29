using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.Features.Doctors.Queries;
using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Doctor_Booking.Application.Features.Doctors.Handlers;

public class GetDoctorDetailsHandler
	: IRequestHandler<GetDoctorDetailsQuery, ResponseViewModel<DoctorDetailsDto>>
{
	private readonly IDoctorBookingDbContext _context;

	public GetDoctorDetailsHandler(IDoctorBookingDbContext context)
	{
		_context = context;
	}
	public async Task<ResponseViewModel<DoctorDetailsDto>> Handle(GetDoctorDetailsQuery request, CancellationToken cancellationToken)
	{
		var doctor = await _context.Doctors
			.Include(d => d.User)  
		   .Include(d => d.Reviews)
		   .FirstOrDefaultAsync(d => d.Id == request.DoctorId);

		if (doctor == null)
			ResponseViewModel<DoctorDetailsDto>.FailureResponse("Doctor not found");
			

		

		var result = new DoctorDetailsDto
		{
			DoctorId = doctor.Id,
			Full_Name = doctor.User.FirstName +" "+doctor.User.LastName ,
			About = doctor.About,
			doctor_Status = doctor.Status,
			SessionPrice = doctor.SessionPrice,
			AverageRating = doctor.Reviews.Any()
				? doctor.Reviews.Average(r => r.Rating)
				: 0,
			Reviews = doctor.Reviews.Select(r => new ReviewDto
			{
				Rating = r.Rating,
				Comment = r.Comment,
				CreatedAt = r.CreatedAt
			}).ToList()
		};
		return ResponseViewModel<DoctorDetailsDto>.SuccessResponse(result);
	}

	
}
