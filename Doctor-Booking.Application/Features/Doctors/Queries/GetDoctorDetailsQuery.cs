using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Doctors.Queries;

public record GetDoctorDetailsQuery(int DoctorId)
	: IRequest<ResponseViewModel<DoctorDetailsDto>>;// request

//result


