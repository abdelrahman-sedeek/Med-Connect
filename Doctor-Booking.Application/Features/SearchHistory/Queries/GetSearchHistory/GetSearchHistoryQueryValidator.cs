using FluentValidation;

namespace Doctor_Booking.Application.Features.SearchHistory.Queries.GetSearchHistory
{
    public class GetSearchHistoryQueryValidator : AbstractValidator<GetSearchHistoryQuery>
    {
        public GetSearchHistoryQueryValidator()
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
        }
    }
}
