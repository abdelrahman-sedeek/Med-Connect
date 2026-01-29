using FluentValidation;

namespace Doctor_Booking.Application.Features.Doctors.Queries.SearchDoctors
{
    public class SearchDoctorsQueryValidator : AbstractValidator<SearchDoctorsQuery>
    {
        public SearchDoctorsQueryValidator()
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

            // Query validation
            RuleFor(x => x.Query)
                .MaximumLength(100)
                .WithMessage("Search query cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Query));

            // Specialty validation
            RuleFor(x => x.Specialty)
                .MaximumLength(50)
                .WithMessage("Specialty cannot exceed 50 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Specialty));

            // Location validation
            RuleFor(x => x.RadiusKm)
                .GreaterThan(0).WithMessage("Radius must be greater than 0.");
            When(x => x.Latitude.HasValue || x.Longitude.HasValue, () =>
            {
                RuleFor(x => x.Latitude)
                    .NotNull().WithMessage("Latitude is required when Longitude is provided.")
                    .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");
                RuleFor(x => x.Longitude)
                    .NotNull().WithMessage("Longitude is required when Latitude is provided.")
                    .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
            });

            // Pagination validation
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(50).WithMessage("Page size cannot exceed 50.");
        }
    }
}
