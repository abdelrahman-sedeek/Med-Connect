using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.AvailabilitySlots.Queries.GetAvailabilitySlotById
{
    public class GetAvailabilitySlotByIdQueryHandler : IRequestHandler<GetAvailabilitySlotByIdQuery, ResponseViewModel<AvailabilitySlotsDto>>
    {
        private readonly IAvailabilitySlotsRepository availabilitySlotsRepository;

        public GetAvailabilitySlotByIdQueryHandler(IAvailabilitySlotsRepository availabilitySlotsRepository)
        {
            this.availabilitySlotsRepository = availabilitySlotsRepository;
        }

        public async Task<ResponseViewModel<AvailabilitySlotsDto>> Handle(GetAvailabilitySlotByIdQuery request, CancellationToken cancellationToken)
        {
            var slot = await availabilitySlotsRepository.GetAvailabilitySlotByIdAsync(request.Id, cancellationToken);
            
            if (slot == null)
            {
                return ResponseViewModel<AvailabilitySlotsDto>.FailureResponse(
                    message: $"Availability slot with ID {request.Id} not found.",
                    status: 404
                );
            }

            return ResponseViewModel<AvailabilitySlotsDto>.SuccessResponse(slot);
        }
    }
}
