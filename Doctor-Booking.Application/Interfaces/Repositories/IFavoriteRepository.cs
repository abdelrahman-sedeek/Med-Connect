using Doctor_Booking.Application.Features.Favorites.Queries.GetFavorites;
using Doctor_Booking.Domain.Entities;

namespace Doctor_Booking.Application.Interfaces.Repositories
{
    public interface IFavoriteRepository
    {
        Task<List<GetFavoritesDto>> GetFavoritesAsync(int patientId, CancellationToken cancellationToken = default);
        Task<Favorite?> GetFavoriteAsync(int patientId, int doctorId, CancellationToken cancellationToken = default);
        Task<bool> DoctorExistsAsync(int doctorId, CancellationToken cancellationToken = default);
        Task<bool> PatientExistsAsync(int patientId, CancellationToken cancellationToken = default);
        Task AddToFavoritesAsync(int patientId, int doctorId, CancellationToken cancellationToken = default);
        Task<bool> RemoveFromFavoritesAsync(int reviewId, CancellationToken cancellationToken = default);
    }
}
