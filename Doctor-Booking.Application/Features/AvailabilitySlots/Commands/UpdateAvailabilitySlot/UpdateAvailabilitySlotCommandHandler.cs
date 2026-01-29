using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.AvailabilitySlots.Commands.UpdateAvailabilitySlot
{
    public class UpdateAvailabilitySlotCommandHandler : IRequestHandler<UpdateAvailabilitySlotCommand, ResponseViewModel<bool>>
    {
        private readonly IAvailabilitySlotsRepository availabilitySlotsRepository;

        public UpdateAvailabilitySlotCommandHandler(IAvailabilitySlotsRepository availabilitySlotsRepository)
        {
            this.availabilitySlotsRepository = availabilitySlotsRepository;
        }

        public async Task<ResponseViewModel<bool>> Handle(UpdateAvailabilitySlotCommand request, CancellationToken cancellationToken)
        {
            // Get the existing slot
            var slot = await availabilitySlotsRepository.GetAvailabilitySlotByIdAsync(request.Id, cancellationToken);
            if (slot == null)
            {
                return ResponseViewModel<bool>.FailureResponse(
                    message: $"Availability slot with ID {request.Id} not found.",
                    status: 404
                );
            }

            // Check if slot is already booked
            if (slot.IsBooked)
            {
                return ResponseViewModel<bool>.FailureResponse(
                    message: "Cannot update a booked availability slot.",
                    status: 409
                );
            }

            // Check for overlapping slots (excluding current slot)
            var hasOverlap = await availabilitySlotsRepository.HasOverlappingSlotsAsync(
                slot.DoctorId,
                request.StartTime,
                request.EndTime,
                cancellationToken,
                request.Id // Exclude current slot
            );

            if (hasOverlap)
            {
                return ResponseViewModel<bool>.FailureResponse(
                    message: "This time slot overlaps with an existing availability slot.",
                    status: 409
                );
            }

            bool result = await availabilitySlotsRepository.UpdateAvailabilitySlotAsync(
                request.Id,
                request.StartTime,
                request.EndTime,
                cancellationToken
            );

            if (!result)
            {
                return ResponseViewModel<bool>.FailureResponse(
                    message: "Failed to update availability slot.",
                    status: 400
                );
            }

            return ResponseViewModel<bool>.SuccessResponse(
                data: true,
                message: "Availability slot updated successfully"
            );
        }
    }
}
