using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.AvailabilitySlots.Queries.GetDoctorAvailabilitySlots
{
    public record GetDoctorAvailabilitySlotsQuery : IRequest<ResponseViewModel<List<AvailabilitySlotsDto>>>
    {
        public int DoctorId { get; init; }
    }
}
