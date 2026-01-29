using Doctor_Booking.Application.Common.Models;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Doctors.Queries.SearchDoctors
{
    public record SearchDoctorsQuery : PaginatedRequest, IRequest<ResponseViewModel<PaginatedList<SearchDoctorsDto>>>
    {
        public string? Query { get; init; }
        public string? Specialty { get; init; }
        public double? Latitude { get; init; }
        public double? Longitude { get; init; }
        public double RadiusKm { get; init; }
        public int? PatientId { get; init; }
        public int? DoctorId { get; init; }
    }
}
