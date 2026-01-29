using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.AvailabilitySlots.Queries.GetDoctorAvailabilitySlots
{
    public class GetDoctorAvailabilitySlotsQueryHandler : IRequestHandler<GetDoctorAvailabilitySlotsQuery, ResponseViewModel<List<AvailabilitySlotsDto>>>
    {
        private readonly IAvailabilitySlotsRepository availabilitySlotsRepository;

        public GetDoctorAvailabilitySlotsQueryHandler(IAvailabilitySlotsRepository availabilitySlotsRepository)
        {
            this.availabilitySlotsRepository = availabilitySlotsRepository;
        }

        public async Task<ResponseViewModel<List<AvailabilitySlotsDto>>> Handle(GetDoctorAvailabilitySlotsQuery request, CancellationToken cancellationToken)
        {
            var slots = await availabilitySlotsRepository.GetDoctorAvailabilitySlotsAsync(request.DoctorId, cancellationToken);
            return ResponseViewModel<List<AvailabilitySlotsDto>>.SuccessResponse(slots);
        }
    }
}
