using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.AvailabiltySlot.Query;

public record GetDoctorAvailabilitySlotsQuery(int DoctorId)
	: IRequest<ResponseViewModel<List<AvailabilitySlotDto>>>;