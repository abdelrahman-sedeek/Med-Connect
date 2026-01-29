using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.AvailabilitySlots.Commands.CreateAvailabilitySlot
{
    public class CreateAvailabilitySlotCommandHandler : IRequestHandler<CreateAvailabilitySlotCommand, ResponseViewModel<int>>
    {
        private readonly IAvailabilitySlotsRepository availabilitySlotsRepository;

        public CreateAvailabilitySlotCommandHandler(IAvailabilitySlotsRepository availabilitySlotsRepository)
        {
            this.availabilitySlotsRepository = availabilitySlotsRepository;
        }

        public async Task<ResponseViewModel<int>> Handle(CreateAvailabilitySlotCommand request, CancellationToken cancellationToken)
        {
            // Check if doctor exists
            var doctorExists = await availabilitySlotsRepository.DoctorExistsAsync(request.DoctorId, cancellationToken);
            if (!doctorExists)
            {
                return ResponseViewModel<int>.FailureResponse(
                    message: $"Doctor with ID {request.DoctorId} not found.",
                    status: 404
                );
            }

            // Check for overlapping slots
            var hasOverlap = await availabilitySlotsRepository.HasOverlappingSlotsAsync(
                request.DoctorId,
                request.StartTime,
                request.EndTime,
                cancellationToken
            );

            if (hasOverlap)
            {
                return ResponseViewModel<int>.FailureResponse(
                    message: "This time slot overlaps with an existing availability slot.",
                    status: 409
                );
            }

            int slotId = await availabilitySlotsRepository.CreateAvailabilitySlotAsync(
                request.StartTime,
                request.EndTime,
                request.DoctorId,
                cancellationToken
            );

            return ResponseViewModel<int>.SuccessResponse(
                data: slotId,
                status: 201,
                message: "Availability slot created successfully"
            );
        }
    }
}
