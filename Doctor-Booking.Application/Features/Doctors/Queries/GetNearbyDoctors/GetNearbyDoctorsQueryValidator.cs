using FluentValidation;

namespace Doctor_Booking.Application.Features.Doctors.Queries.GetNearbyDoctors
{
    public class GetNearbyDoctorsQueryValidator : AbstractValidator<GetNearbyDoctorsQuery>
    {
        public GetNearbyDoctorsQueryValidator()
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

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage("Latitude must be between -90 and 90 degrees.");

            RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180 degrees.");

            RuleFor(x => x.RadiusKm)
            .GreaterThan(0)
            .WithMessage("Radius must be greater than 0 km.")
            .LessThanOrEqualTo(100)
            .WithMessage("Radius cannot exceed 100 km for performance reasons.");

            // Pagination validation
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page number must be at least 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page size must be at least 1.")
                .LessThanOrEqualTo(50)
                .WithMessage("Page size cannot exceed 50.");
        }
    }
}
