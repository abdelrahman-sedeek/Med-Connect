using Doctor_Booking.Application.Features.SearchHistory.Queries.GetSearchHistory;
using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Domain.Entities;
using Doctor_Booking.Infastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Doctor_Booking.Infastructure.Repositories
{
    public class SearchHistoryRepository : ISearchHistoryRepository
    {
        private readonly DoctorBookingDbContext context;
        public SearchHistoryRepository(DoctorBookingDbContext context)
        {
            this.context = context;
        }


        public async Task<bool> DeleteSearchHistoryAsync(int id, CancellationToken cancellationToken = default)
        {
            var history = await context.SearchHistorys.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
            if (history == null) return false;

            context.SearchHistorys.Remove(history);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<List<GetSearchHistoryDto>> GetSearchHistoryAsync(int? patientId, int? doctorId, int limit, CancellationToken cancellationToken = default)
        {
            var query = context.SearchHistorys.AsQueryable();
            if(patientId.HasValue)
                query = query.Where(s => s.PatientId == patientId.Value);

            if (doctorId.HasValue)
                query = query.Where(s => s.DoctorId == doctorId.Value);

            return await query
                .OrderByDescending(s => s.CreatedAt)
                .Take(limit)
                .Select(s => new GetSearchHistoryDto
                {
                    Id = s.Id,
                    Query = s.Keyword,
                    Specialty = s.Specialty,
                    Location = s.Location,
                    CreatedAt = s.CreatedAt
                }).ToListAsync(cancellationToken);
        }

        public async Task SaveSearchHistoryAsync(int? patientId, int? doctorId, string? query, string? specialty, string? location, CancellationToken cancellationToken = default)
        {
            // Build keyword from search criteria
            var keywordParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(query))
                keywordParts.Add(query.Trim());
            if (!string.IsNullOrWhiteSpace(specialty))
                keywordParts.Add(specialty.Trim());
            if (!string.IsNullOrWhiteSpace(location))
                keywordParts.Add(location.Trim());

            var keyword = string.Join(" | ", keywordParts);

            // Check if last search is identical
            var lastSearchQuery = context.SearchHistorys.AsQueryable();

            if(patientId.HasValue)
                lastSearchQuery = lastSearchQuery.Where(s => s.PatientId == patientId.Value);

            if(doctorId.HasValue)
                lastSearchQuery = lastSearchQuery.Where(s => s.DoctorId == doctorId.Value);

            var lastSearch = await lastSearchQuery
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (lastSearch != null && lastSearch.Keyword.Equals(keyword, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // Save new search history
            var searchHistory = new SearchHistory
            {
                PatientId = patientId,
                DoctorId = doctorId,
                Keyword = keyword,
                Specialty = specialty?.Trim(),
                Location = location?.Trim(),
                CreatedAt = DateTime.UtcNow,
            };
            await context.SearchHistorys.AddAsync(searchHistory, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
