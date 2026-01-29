using Doctor_Booking.Application.Features.Favorites.Queries.GetFavorites;
using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Domain.Entities;
using Doctor_Booking.Infastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Doctor_Booking.Infastructure.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly DoctorBookingDbContext context;

        public FavoriteRepository(DoctorBookingDbContext context)
        {
            this.context = context;
        }

        public async Task AddToFavoritesAsync(int patientId, int doctorId, CancellationToken cancellationToken = default)
        {
            await context.Favorites.AddAsync(new Favorite
            {
                PatientId = patientId,
                DoctorId = doctorId,
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public Task<bool> DoctorExistsAsync(int doctorId, CancellationToken cancellationToken = default)
        {
            return context.Doctors.AnyAsync(d => d.Id == doctorId, cancellationToken);
        }

        public async Task<Favorite?> GetFavoriteAsync(int patientId, int doctorId, CancellationToken cancellationToken = default)
        {
            return await context.Favorites.FirstOrDefaultAsync(f => f.PatientId == patientId && f.DoctorId == doctorId, cancellationToken) ?? null;
        }

        public async Task<List<GetFavoritesDto>> GetFavoritesAsync(int patientId, CancellationToken cancellationToken = default)
        {
            var currentDateTime = DateTime.UtcNow;

            return await context.Favorites
            .Where(f => f.PatientId == patientId)
            .Include(f => f.Doctor)
                .ThenInclude(d => d.User)
            .Include(f => f.Doctor)
                .ThenInclude(d => d.Specialty)
            .Include(f => f.Doctor)
                .ThenInclude(d => d.Reviews)
            .Include(f => f.Doctor)
                .ThenInclude(d => d.AvailabilitySlots)
            .OrderByDescending(f => f.CreatedAt)
            .Select(f => new GetFavoritesDto
            {
                DoctorId = f.DoctorId,
                FullName = f.Doctor.User != null
                    ? $"{f.Doctor.User.FirstName} {f.Doctor.User.LastName}".Trim()
                    : "Unknown",
                Specialty = f.Doctor.Specialty != null
                    ? f.Doctor.Specialty.Name
                    : "General",
                ClinicName = f.Doctor.ClinicName,
                Rating = f.Doctor.Reviews.Any()
                    ? Math.Round(f.Doctor.Reviews.Average(r => r.Rating), 1)
                    : 0.0m,
                ImageUrl = f.Doctor.User != null
                    ? f.Doctor.User.ProfileImageUrl ?? "/images/default-doctor.png"
                    : "/images/default-doctor.png",

                AvailableStartDate = f.Doctor.AvailabilitySlots
                    .Where(slot => slot.StartTime >= currentDateTime && !slot.IsBooked)
                    .OrderBy(slot => slot.StartTime)
                    .Select(slot => slot.StartTime)
                    .FirstOrDefault(),

                AvailableEndDate = f.Doctor.AvailabilitySlots
                    .Where(slot => slot.EndTime >= currentDateTime && !slot.IsBooked)
                    .OrderByDescending(slot => slot.EndTime)
                    .Select(slot => slot.EndTime)
                    .FirstOrDefault(),
            })
            .ToListAsync(cancellationToken);
        }

        public async Task<bool> PatientExistsAsync(int patientId, CancellationToken cancellationToken = default)
        {
            return await context.Patients.AnyAsync(p => p.Id == patientId, cancellationToken);
        }

        public async Task<bool> RemoveFromFavoritesAsync(int reviewId, CancellationToken cancellationToken = default)
        {
            var favorite = await context.Favorites.FirstOrDefaultAsync(f => f.Id == reviewId, cancellationToken);
            if (favorite == null)
            {
                return false;
            }
            context.Favorites.Remove(favorite);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
