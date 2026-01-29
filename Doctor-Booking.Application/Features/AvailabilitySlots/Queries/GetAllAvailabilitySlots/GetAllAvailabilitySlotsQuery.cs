using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.AvailabilitySlots.Queries.GetAllAvailabilitySlots
{
    public record GetAllAvailabilitySlotsQuery : IRequest<ResponseViewModel<List<AvailabilitySlotsDto>>>
    {
    }
}
