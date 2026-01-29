using Doctor_Booking.Application.Common.Models;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Doctors.Queries.GetNearbyDoctors
{
    public record GetNearbyDoctorsQuery : PaginatedRequest, IRequest<ResponseViewModel<PaginatedList<NearbyDoctorDto>>>
    {
        public int? PatientId { get; init; }
        public int? DoctorId { get; init; }
        public double? Latitude { get; init; }
        public double? Longitude { get; init; }
        public double RadiusKm { get; init; } = 10;
    }
}
