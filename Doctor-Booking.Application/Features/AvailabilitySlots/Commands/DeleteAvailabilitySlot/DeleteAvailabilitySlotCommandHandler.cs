using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.AvailabilitySlots.Commands.DeleteAvailabilitySlot
{
    public class DeleteAvailabilitySlotCommandHandler : IRequestHandler<DeleteAvailabilitySlotCommand, ResponseViewModel<bool>>
    {
        private readonly IAvailabilitySlotsRepository availabilitySlotsRepository;

        public DeleteAvailabilitySlotCommandHandler(IAvailabilitySlotsRepository availabilitySlotsRepository)
        {
            this.availabilitySlotsRepository = availabilitySlotsRepository;
        }

        public async Task<ResponseViewModel<bool>> Handle(DeleteAvailabilitySlotCommand request, CancellationToken cancellationToken)
        {
            // Get the slot
            var slot = await availabilitySlotsRepository.GetAvailabilitySlotByIdAsync(request.Id, cancellationToken);
            if (slot == null)
            {
                return ResponseViewModel<bool>.FailureResponse(
                    message: $"Availability slot with ID {request.Id} not found.",
                    status: 404
                );
            }

            // Check if slot is booked
            if (slot.IsBooked)
            {
                return ResponseViewModel<bool>.FailureResponse(
                    message: "Cannot delete a booked availability slot.",
                    status: 409
                );
            }

            bool result = await availabilitySlotsRepository.DeleteAvailabilitySlotAsync(request.Id, cancellationToken);

            if (!result)
            {
                return ResponseViewModel<bool>.FailureResponse(
                    message: "Failed to delete availability slot.",
                    status: 400
                );
            }

            return ResponseViewModel<bool>.SuccessResponse(
                data: true,
                message: "Availability slot deleted successfully"
            );
        }
    }
}
