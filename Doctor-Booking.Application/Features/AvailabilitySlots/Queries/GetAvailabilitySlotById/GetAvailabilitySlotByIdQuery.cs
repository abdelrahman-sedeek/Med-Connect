using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.AvailabilitySlots.Queries.GetAvailabilitySlotById
{
    public record GetAvailabilitySlotByIdQuery : IRequest<ResponseViewModel<AvailabilitySlotsDto>>
    {
        public int Id { get; init; }
    }
}
