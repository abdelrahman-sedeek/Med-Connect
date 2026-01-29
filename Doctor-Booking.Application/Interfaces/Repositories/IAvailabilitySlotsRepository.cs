using Doctor_Booking.Application.DTOs;

namespace Doctor_Booking.Application.Interfaces.Repositories
{
    public interface IAvailabilitySlotsRepository
    {
        Task<List<AvailabilitySlotsDto>> GetAllAvailabilitySlotsAsync(CancellationToken cancellationToken = default);
        Task<AvailabilitySlotsDto?> GetAvailabilitySlotByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<AvailabilitySlotsDto>> GetDoctorAvailabilitySlotsAsync(int doctorId, CancellationToken cancellationToken = default);
        Task<int> CreateAvailabilitySlotAsync(DateTime startTime, DateTime endTime, int doctorId, CancellationToken cancellationToken = default);
        Task<bool> UpdateAvailabilitySlotAsync(int id, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default);
        Task<bool> DeleteAvailabilitySlotAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> DoctorExistsAsync(int doctorId, CancellationToken cancellationToken = default);
        Task<bool> HasOverlappingSlotsAsync(int doctorId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default, int? excludeSlotId = null);
    }
}
