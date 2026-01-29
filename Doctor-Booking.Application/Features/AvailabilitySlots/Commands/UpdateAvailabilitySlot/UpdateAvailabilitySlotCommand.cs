using Doctor_Booking.Application.ViewModels;
using FluentValidation;
using MediatR;

namespace Doctor_Booking.Application.Features.AvailabilitySlots.Commands.UpdateAvailabilitySlot
{
    public record UpdateAvailabilitySlotCommand : IRequest<ResponseViewModel<bool>>
    {
        public int Id { get; init; }
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
    }

    public class UpdateAvailabilitySlotValidator : AbstractValidator<UpdateAvailabilitySlotCommand>
    {
        public UpdateAvailabilitySlotValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Availability slot ID must be greater than 0.");

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .WithMessage("Start time is required.")
                .Must(time => time > DateTime.UtcNow)
                .WithMessage("Start time must be in the future.");

            RuleFor(x => x.EndTime)
                .NotEmpty()
                .WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime)
                .WithMessage("End time must be after start time.");

            RuleFor(x => x)
                .Must(x => (x.EndTime - x.StartTime).TotalHours >= 0.5 && (x.EndTime - x.StartTime).TotalHours <= 2)
                .WithMessage("Slot duration must be between 30 minutes and 2 hours.");
        }
    }
}
