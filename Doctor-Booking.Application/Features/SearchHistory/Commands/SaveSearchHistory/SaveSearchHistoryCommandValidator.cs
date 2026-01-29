using FluentValidation;

namespace Doctor_Booking.Application.Features.SearchHistory.Commands.SaveSearchHistory
{
    public class SaveSearchHistoryCommandValidator : AbstractValidator<SaveSearchHistoryCommand>
    {
        public SaveSearchHistoryCommandValidator()
        {
            // Either PatientId OR DoctorId must be provided
            RuleFor(x => x)
                .Must(x => x.PatientId.HasValue || x.DoctorId.HasValue)
                .WithMessage("Either PatientId or DoctorId must be provided.");

            RuleFor(x => x)
                .Must(x => !(x.PatientId.HasValue && x.DoctorId.HasValue))
                .WithMessage("Cannot provide both PatientId and DoctorId.");

            RuleFor(x => x.PatientId)
                .GreaterThan(0)
                .When(x => x.PatientId.HasValue);

            RuleFor(x => x.DoctorId)
                .GreaterThan(0)
                .When(x => x.DoctorId.HasValue);

            RuleFor(x => x.Query)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.Query));

            RuleFor(x => x.Specialty)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Specialty));

            RuleFor(x => x.Location)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.Location));

            // At least one field must be provided
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.Query) ||
                          !string.IsNullOrWhiteSpace(x.Specialty) ||
                          !string.IsNullOrWhiteSpace(x.Location))
                .WithMessage("At least one search criteria (query, specialty, or location) must be provided.");
        }
    }
}
