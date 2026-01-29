using Doctor_Booking.Application.Features.Specialties.Queries.GetAllSpecialties;
using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Domain.Entities;
using Doctor_Booking.Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Infastructure.Repositories
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly DoctorBookingDbContext context;
        public SpecialtyRepository(DoctorBookingDbContext context)
        {
            this.context = context;
        }


        public async Task<int> CreateSpecialtyAsync(string Name, CancellationToken cancellationToken = default)
        {
            Specialty specialty = new Specialty
            {
                Name = Name.Trim()
            };
            await context.Specialtys.AddAsync(specialty, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return specialty.Id;
        }

        public async Task<bool> DeleteSpecialtyAsync(int id, CancellationToken cancellationToken = default)
        {
            Specialty? specialty = context.Specialtys.FirstOrDefault(s => s.Id == id);
            if (specialty == null)
            {
                return false;
            }
            context.Specialtys.Remove(specialty);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<List<SpecialtyDto>> GetAllSpecialtyAsync(CancellationToken cancellationToken = default)
        {
            var specialties = await context.Specialtys.Select(s => new SpecialtyDto
            {
                Id = s.Id,
                Name = s.Name
            }).OrderBy(s => s.Name).ToListAsync(cancellationToken);

            return specialties;
        }

        public async Task<SpecialtyDto?> GetSpecialtyByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.Specialtys
            .Where(s => s.Id == id)
            .Select(s => new SpecialtyDto
            {
                Id = s.Id,
                Name = s.Name,
            })
            .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> SpecialtyExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            return await context.Specialtys.AnyAsync(s => s.Name == name, cancellationToken);
        }

        public async Task<bool> SpecialtyHasDoctorsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.Doctors.AnyAsync(d => d.SpecialtyId == id, cancellationToken);
        }

        public async Task<bool> UpdateSpecialtyAsync(int id, string name, CancellationToken cancellationToken = default)
        {
            Specialty? specialty = await context.Specialtys.FirstOrDefaultAsync(s => s.Id == id);
            if(specialty == null)
            {
                return false;
            }

            specialty.Name = name.Trim();
            specialty.CreatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
