using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Domain.Entities;
using Doctor_Booking.Infastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Doctor_Booking.Infastructure.Repositories
{
    public class AvailabilitySlotsRepository : IAvailabilitySlotsRepository
    {
        private readonly DoctorBookingDbContext context;

        public AvailabilitySlotsRepository(DoctorBookingDbContext context)
        {
            this.context = context;
        }

        public async Task<List<AvailabilitySlotsDto>> GetAllAvailabilitySlotsAsync(CancellationToken cancellationToken = default)
        {
            return await context.AvailabilitySlots
                .Select(s => new AvailabilitySlotsDto
                {
                    Id = s.Id,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    IsBooked = s.IsBooked,
                    DoctorId = s.DoctorId ?? 0
                })
                .OrderBy(s => s.StartTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<AvailabilitySlotsDto?> GetAvailabilitySlotByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.AvailabilitySlots
                .Where(s => s.Id == id)
                .Select(s => new AvailabilitySlotsDto
                {
                    Id = s.Id,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    IsBooked = s.IsBooked,
                    DoctorId = s.DoctorId ?? 0
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<AvailabilitySlotsDto>> GetDoctorAvailabilitySlotsAsync(int doctorId, CancellationToken cancellationToken = default)
        {
            return await context.AvailabilitySlots
                .Where(s => s.DoctorId == doctorId)
                .Select(s => new AvailabilitySlotsDto
                {
                    Id = s.Id,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    IsBooked = s.IsBooked,
                    DoctorId = s.DoctorId ?? 0
                })
                .OrderBy(s => s.StartTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CreateAvailabilitySlotAsync(DateTime startTime, DateTime endTime, int doctorId, CancellationToken cancellationToken = default)
        {
            var availabilitySlot = new AvailabilitySlot
            {
                StartTime = startTime,
                EndTime = endTime,
                IsBooked = false,
                DoctorId = doctorId
            };

            await context.AvailabilitySlots.AddAsync(availabilitySlot, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return availabilitySlot.Id;
        }

        public async Task<bool> UpdateAvailabilitySlotAsync(int id, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default)
        {
            var availabilitySlot = await context.AvailabilitySlots.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
            if (availabilitySlot == null)
            {
                return false;
            }

            availabilitySlot.StartTime = startTime;
            availabilitySlot.EndTime = endTime;

            context.AvailabilitySlots.Update(availabilitySlot);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteAvailabilitySlotAsync(int id, CancellationToken cancellationToken = default)
        {
            var availabilitySlot = await context.AvailabilitySlots.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
            if (availabilitySlot == null)
            {
                return false;
            }

            context.AvailabilitySlots.Remove(availabilitySlot);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DoctorExistsAsync(int doctorId, CancellationToken cancellationToken = default)
        {
            return await context.Doctors.AnyAsync(d => d.Id == doctorId, cancellationToken);
        }

        public async Task<bool> HasOverlappingSlotsAsync(int doctorId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default, int? excludeSlotId = null)
        {
            var query = context.AvailabilitySlots
                .Where(s => s.DoctorId == doctorId &&
                           s.StartTime < endTime &&
                           s.EndTime > startTime);

            if (excludeSlotId.HasValue)
            {
                query = query.Where(s => s.Id != excludeSlotId.Value);
            }

            return await query.AnyAsync(cancellationToken);
        }
    }
}
