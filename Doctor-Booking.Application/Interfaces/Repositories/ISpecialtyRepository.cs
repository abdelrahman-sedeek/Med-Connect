using Doctor_Booking.Application.Features.Specialties.Queries.GetAllSpecialties;

namespace Doctor_Booking.Application.Interfaces.Repositories
{
    public interface ISpecialtyRepository
    {
        Task<List<SpecialtyDto>> GetAllSpecialtyAsync(CancellationToken cancellationToken = default);
        Task<SpecialtyDto?> GetSpecialtyByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateSpecialtyAsync(string Name, CancellationToken cancellationToken = default);
        Task<bool> UpdateSpecialtyAsync(int id, string name, CancellationToken cancellationToken = default);
        Task<bool> DeleteSpecialtyAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> SpecialtyExistsAsync(string name, CancellationToken cancellationToken = default);
        Task<bool> SpecialtyHasDoctorsAsync(int id, CancellationToken cancellationToken = default);
    }
}
