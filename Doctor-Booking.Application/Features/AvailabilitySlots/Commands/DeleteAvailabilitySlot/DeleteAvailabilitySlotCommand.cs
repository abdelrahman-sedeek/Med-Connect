using Doctor_Booking.Application.ViewModels;
using FluentValidation;
using MediatR;

namespace Doctor_Booking.Application.Features.AvailabilitySlots.Commands.DeleteAvailabilitySlot
{
    public record DeleteAvailabilitySlotCommand : IRequest<ResponseViewModel<bool>>
    {
        public int Id { get; init; }
    }

    public class DeleteAvailabilitySlotValidator : AbstractValidator<DeleteAvailabilitySlotCommand>
    {
        public DeleteAvailabilitySlotValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Availability slot ID must be greater than 0.");
        }
    }
}
