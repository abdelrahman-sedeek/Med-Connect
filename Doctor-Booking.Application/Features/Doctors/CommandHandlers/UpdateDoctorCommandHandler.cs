using Doctor_Booking.Application.DTOs.Doctor;
using Doctor_Booking.Application.Features.Doctors.Commands;
using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.Doctors.CommandHandlers
{
    public class UpdateDoctorCommandHandler
         : IRequestHandler<UpdateDoctorCommand, ResponseViewModel<DoctorDto>>
    {
        private readonly IDoctorBookingDbContext _context;
        private readonly ILogger<UpdateDoctorCommandHandler> _logger;
        private readonly GeometryFactory geometryFactory;

        public UpdateDoctorCommandHandler(
            IDoctorBookingDbContext context,
        
            ILogger<UpdateDoctorCommandHandler> logger)
        {
            geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseViewModel<DoctorDto>> Handle(
            UpdateDoctorCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.dto;
                // Step 1: Create Point from user's location
                // IMPORTANT: Coordinate(longitude, latitude) - X before Y!
                var userLocation = geometryFactory.CreatePoint(new Coordinate(dto.longitude, dto.latitude));

                var doctor = await _context.Doctors
                    .Include(d => d.User)
                    .FirstOrDefaultAsync(d => d.Id == dto.Id, cancellationToken);

                if (doctor == null)
                {
                    return ResponseViewModel<DoctorDto>.FailureResponse(
                        message: "Doctor not found",
                        status: 404
                    );
                }

                // Update ApplicationUser
                doctor.User.FirstName = dto.FirstName;
                doctor.User.LastName = dto.LastName;
                doctor.User.ProfileImageUrl = dto.ProfileImageUrl;
                doctor.SpecialtyId = dto.SpecialtyId;
                doctor.About = dto.About;
                doctor.ClinicName = dto.ClinicName;
                doctor.SessionPrice = dto.SessionPrice;
                doctor.Location = userLocation;
               

                await _context.SaveChangesAsync(cancellationToken);

                var doctorDto = MapToDto(doctor);

                return ResponseViewModel<DoctorDto>.SuccessResponse(
                    data: doctorDto,
                    message: "Doctor updated successfully",
                    status: 200
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating doctor");
                return ResponseViewModel<DoctorDto>.FailureResponse(
                    message: "An error occurred while updating doctor",
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
                ClinicName= doctor.ClinicName,
                SessionPrice = doctor.SessionPrice,
                langitude = doctor.Location.X,
                latitude = doctor.Location.Y,
                CreatedAt = doctor.CreatedAt
            };
        }
    
    
    }
}
