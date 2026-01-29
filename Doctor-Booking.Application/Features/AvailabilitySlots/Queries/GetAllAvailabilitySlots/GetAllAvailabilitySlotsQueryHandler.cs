using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.AvailabilitySlots.Queries.GetAllAvailabilitySlots
{
    public class GetAllAvailabilitySlotsQueryHandler : IRequestHandler<GetAllAvailabilitySlotsQuery, ResponseViewModel<List<AvailabilitySlotsDto>>>
    {
        private readonly IAvailabilitySlotsRepository availabilitySlotsRepository;

        public GetAllAvailabilitySlotsQueryHandler(IAvailabilitySlotsRepository availabilitySlotsRepository)
        {
            this.availabilitySlotsRepository = availabilitySlotsRepository;
        }

        public async Task<ResponseViewModel<List<AvailabilitySlotsDto>>> Handle(GetAllAvailabilitySlotsQuery request, CancellationToken cancellationToken)
        {
            var slots = await availabilitySlotsRepository.GetAllAvailabilitySlotsAsync(cancellationToken);
            return ResponseViewModel<List<AvailabilitySlotsDto>>.SuccessResponse(slots);
        }
    }
}
