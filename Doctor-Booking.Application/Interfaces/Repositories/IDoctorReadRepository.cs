using Doctor_Booking.Application.Common.Models;
using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.Features.Doctors.Queries.GetNearbyDoctors;
using Doctor_Booking.Application.Features.Doctors.Queries.SearchDoctors;

namespace Doctor_Booking.Application.Interfaces.Repositories
{
    public interface IDoctorReadRepository
    {
        Task<PaginatedList<NearbyDoctorDto>> GetNearbyDoctorsAsync(double lat, double lng, double radiusKm, int pageNumber, int pageSize);
        Task<PaginatedList<SearchDoctorsDto>> SearchDoctorsAsync(string? query, string? specialty, double latitude, double longitude, double radiusKm, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<LocationDto?> GetDoctorLocationAsync(int doctorId, CancellationToken cancellationToken = default);
    }
}
