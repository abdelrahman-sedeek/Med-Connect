using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Infastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Doctor_Booking.Infastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DoctorBookingDbContext context;

        public PatientRepository(DoctorBookingDbContext context)
        {
            this.context = context;
        }


        public async Task<LocationDto?> GetPatientLocationAsync(int patientId, CancellationToken cancellationToken = default)
        {
            var patient = await context.Patients
                .Where(p => p.Id == patientId && p.Location != null)
                .Select(p => new LocationDto
                {
                    Latitude = p.Location!.Y,  // Y = Latitude
                    Longitude = p.Location.X  // X = Longitude
                })
                .FirstOrDefaultAsync(cancellationToken);

            return patient;
        }
    }
}
