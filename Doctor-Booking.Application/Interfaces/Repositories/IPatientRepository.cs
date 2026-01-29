using Doctor_Booking.Application.DTOs;

namespace Doctor_Booking.Application.Interfaces.Repositories
{
    public interface IPatientRepository
    {
        Task<LocationDto?> GetPatientLocationAsync(int patientId, CancellationToken cancellationToken = default);
    }
}
