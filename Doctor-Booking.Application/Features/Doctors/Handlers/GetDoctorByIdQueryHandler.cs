using Doctor_Booking.Application.DTOs.Doctor;
using Doctor_Booking.Application.Features.Doctors.Queries;
using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;


namespace Doctor_Booking.Application.Features.Doctors.Handlers
{
    public class GetDoctorByIdQueryHandler
         : IRequestHandler<GetDoctorByIdQuery, ResponseViewModel<DoctorDto>>
    {
        private readonly IDoctorBookingDbContext _context;
        private readonly ILogger<GetDoctorByIdQueryHandler> _logger;

        public GetDoctorByIdQueryHandler(
            IDoctorBookingDbContext context,
            ILogger<GetDoctorByIdQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseViewModel<DoctorDto>> Handle(
            GetDoctorByIdQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var doctor = await _context.Doctors
                    .Include(d => d.User)
                    .Include(d => d.AvailabilitySlots)
                    .FirstOrDefaultAsync(d => d.User.IsActive == true && d.Id == request.id  , cancellationToken);

                if (doctor == null)
                {
                    return ResponseViewModel<DoctorDto>.FailureResponse(
                        message: "Doctor not found",
                        status: 404
                    );
                }

                var doctorDto = MapToDto(doctor);

                return ResponseViewModel<DoctorDto>.SuccessResponse(
                    data: doctorDto,
                    message: "Doctor retrieved successfully",
                    status: 200
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving doctor");
                return ResponseViewModel<DoctorDto>.FailureResponse(
                    message: "An error occurred",
                    status: 500,
                    errors: new List<object> { ex.Message }
                );
            }
        }

        private DoctorDto MapToDto(Doctor doctor)
        {
            return new DoctorDto
            {
                Id = doctor.Id,
                UserId = doctor.UserId,
                FirstName = doctor.User.FirstName,
                LastName = doctor.User.LastName,
                Email = doctor.User.Email ?? "",
                PhoneNumber = doctor.User.PhoneNumber ?? "",
                ProfileImageUrl = doctor.User.ProfileImageUrl,
                SpecialtyId = doctor.SpecialtyId,
                LicenseNumber = doctor.LicenseNumber,
                About = doctor.About,
                ClinicName = doctor.ClinicName,
                SessionPrice = doctor.SessionPrice,
                latitude = doctor.Location.Y ,
                langitude = doctor.Location.X ,
                IsActive = doctor.User.IsActive,
                CreatedAt = doctor.CreatedAt,
                Availabilities = doctor.AvailabilitySlots.Select(a => new DoctorAvailabilityDto
                {
                    Id = a.Id,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    IsBooked = a.IsBooked
                }).ToList()
            };
        }
    }

}
