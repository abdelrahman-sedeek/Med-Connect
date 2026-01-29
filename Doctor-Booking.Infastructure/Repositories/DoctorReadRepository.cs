using Doctor_Booking.Application.Common.Models;
using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.Features.Doctors.Queries.GetNearbyDoctors;
using Doctor_Booking.Application.Features.Doctors.Queries.SearchDoctors;
using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Threading;

namespace Doctor_Booking.Infastructure.Repositories
{
    public class DoctorReadRepository : IDoctorReadRepository
    {
        private readonly DoctorBookingDbContext context;
        private readonly GeometryFactory geometryFactory;
        public DoctorReadRepository(DoctorBookingDbContext context)
        {
            this.context = context;
            geometryFactory = new GeometryFactory(new PrecisionModel(), 4326); // SRID 4326 = WGS84 (standard GPS coordinates)
        }

        public async Task<LocationDto?> GetDoctorLocationAsync(int doctorId, CancellationToken cancellationToken = default)
        {
            var doctor = await context.Doctors
                .Where(d => d.Id == doctorId && d.Location != null)
                .Select(d => new LocationDto
                {
                    Latitude = d.Location.Y,   // Y = Latitude
                    Longitude = d.Location.X   // X = Longitude
                })
                .FirstOrDefaultAsync(cancellationToken);

            return doctor;
        }

        public async Task<PaginatedList<NearbyDoctorDto>> GetNearbyDoctorsAsync(double lat, double lng, double radiusKm, int pageNumber, int pageSize)
        {
            // Step 1: Create Point from user's location
            // IMPORTANT: Coordinate(longitude, latitude) - X before Y!
            var userLocation = geometryFactory.CreatePoint(new Coordinate(lng, lat));

            // Step 2: Convert radius from kilometers to meters (SQL Server uses meters)
            var radiusMeters = radiusKm * 1000;

            var currentDateTime = DateTime.UtcNow;

            var totalCount = await context.Doctors.Where(d => d.IsDoctorApproved && d.UserId != null && d.Location.Distance(userLocation) <= radiusMeters).CountAsync();

            // Step 3: Query doctors within the specified radius
            var nearbyDoctors = await context.Doctors
                .Where(d =>
                    d.IsDoctorApproved &&
                    d.UserId != null &&
                    d.Location.IsWithinDistance(userLocation, radiusMeters))
                .Include(d => d.User)
                .Include(d => d.Specialty)
                .Include(d => d.Reviews)
                .Include(d => d.AvailabilitySlots.Where(slot => slot.StartTime >= currentDateTime && !slot.IsBooked)) // Only future available slots
                .OrderBy(d => d.Location.Distance(userLocation))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new NearbyDoctorDto
                {
                    Id = d.Id,

                    FullName = d.User != null ? $"{d.User.FirstName} {d.User.LastName}".Trim() : "Unknown",

                    Specialty = d.Specialty != null ? d.Specialty.Name : "General",

                    ClinicName = d.ClinicName,

                    Rating = d.Reviews.Any() ? Math.Round(d.Reviews.Average(r => r.Rating), 1) : 0.0m,

                    ImageUrl = d.User != null ? d.User.ProfileImageUrl : null,

                    AvailableStartDate = d.AvailabilitySlots
                    .Where(slot => slot.StartTime >= currentDateTime && !slot.IsBooked)
                    .OrderBy(slot => slot.StartTime)
                    .Select(slot => slot.StartTime)
                    .FirstOrDefault(),

                    AvailableEndDate = d.AvailabilitySlots
                    .Where(slot => slot.EndTime >= currentDateTime && !slot.IsBooked)
                    .OrderByDescending(slot => slot.EndTime)
                    .Select(slot => slot.EndTime)
                    .FirstOrDefault(),
                })
                .ToListAsync();

            return new PaginatedList<NearbyDoctorDto>(nearbyDoctors, totalCount, pageNumber, pageSize);
        }

        public async Task<PaginatedList<SearchDoctorsDto>> SearchDoctorsAsync(string? query, string? specialty, double latitude, double longitude, double radiusKm, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var searchQuery = context.Doctors.Where(d => d.IsDoctorApproved && d.UserId != null).AsQueryable();

            // Filter by name (doctor's first/last name)
            if (!string.IsNullOrWhiteSpace(query))
            {
                var searchTerm = query.Trim().ToLower();
                searchQuery = searchQuery.Where(d =>
                    d.User != null &&
                    (d.User.FirstName.ToLower().Contains(searchTerm) ||
                     d.User.LastName.ToLower().Contains(searchTerm) ||
                     (d.User.FirstName + " " + d.User.LastName).ToLower().Contains(searchTerm))
                );
            }

            // Filter by specialty
            if (!string.IsNullOrWhiteSpace(specialty))
            {
                var specialtyTerm = specialty.Trim().ToLower();
                searchQuery = searchQuery.Where(d =>
                    d.Specialty != null &&
                    d.Specialty.Name.ToLower().Contains(specialtyTerm)
                );
            }

            // Filter by distance
            var userLocation = geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
            var radiusMeters = radiusKm * 1000;
            searchQuery = searchQuery.Where(d =>
                d.Location != null &&
                d.Location.IsWithinDistance(userLocation, radiusMeters)
            );

            // Get total count before pagination
            var totalCount = await searchQuery.CountAsync(cancellationToken);

            // Map to DTO with pagination
            var doctors = await searchQuery
                .Include(d => d.User)
                .Include(d => d.Specialty)
                .Include(d => d.Reviews)
                .Include(d => d.AvailabilitySlots)
                //.OrderBy(d => d.Location.Distance(userLocation))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new SearchDoctorsDto
                {
                    Id = d.Id,
                    FullName = d.User != null ? $"{d.User.FirstName} {d.User.LastName}".Trim() : "Unknown",
                    Specialty = d.Specialty != null ? d.Specialty.Name : "General",
                    ClinicName = d.ClinicName,
                    Rating = d.Reviews.Any() ? Math.Round(d.Reviews.Average(r => r.Rating), 1) : 0.0m,
                    ImageUrl = d.User != null ? d.User.ProfileImageUrl : null,
                    AvailableStartDate = d.AvailabilitySlots
                        .Where(slot => slot.StartTime >= DateTime.UtcNow && !slot.IsBooked)
                        .OrderBy(slot => slot.StartTime)
                        .Select(slot => slot.StartTime)
                        .FirstOrDefault(),
                    AvailableEndDate = d.AvailabilitySlots
                        .Where(slot => slot.EndTime >= DateTime.UtcNow && !slot.IsBooked)
                        .OrderByDescending(slot => slot.EndTime)
                        .Select(slot => slot.EndTime)
                        .FirstOrDefault(),
                })
                .ToListAsync(cancellationToken);

            return new PaginatedList<SearchDoctorsDto>(doctors, totalCount, pageNumber, pageSize);
        }
    }
}
