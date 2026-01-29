using Doctor_Booking.Application.Features.SearchHistory.Queries.GetSearchHistory;

namespace Doctor_Booking.Application.Interfaces.Repositories
{
    public interface ISearchHistoryRepository
    {
        Task SaveSearchHistoryAsync(int? patientId, int? doctorId, string? query, string? specialty, string? location, CancellationToken cancellationToken = default);
        Task<List<GetSearchHistoryDto>> GetSearchHistoryAsync(int? patientId, int? doctorId, int limit, CancellationToken cancellationToken = default);
        Task<bool> DeleteSearchHistoryAsync(int id, CancellationToken cancellationToken = default);
    }
}
