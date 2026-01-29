using Doctor_Booking.Application.DTOs.Doctor;
using Doctor_Booking.Application.Features.Doctors.Commands;
using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Application.Interfaces.Services;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.Doctors.CommandHandlers
{
    public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, ResponseViewModel<DoctorDto>>
    {
        private readonly IDoctorBookingDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISmsService _smsService;
        private readonly ILogger<CreateDoctorCommandHandler> _logger;
        private readonly GeometryFactory geometryFactory;

        public CreateDoctorCommandHandler(
            IDoctorBookingDbContext context,
            UserManager<ApplicationUser> userManager,
            ISmsService smsService,
          
            ILogger<CreateDoctorCommandHandler> logger)
        {
             geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);
            _context = context;
            _userManager = userManager;
            _smsService = smsService;
            _logger = logger;
        }
        public async Task<ResponseViewModel<DoctorDto>> Handle(CreateDoctorCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                // Step 1: Create Point from user's location
                // IMPORTANT: Coordinate(longitude, latitude) - X before Y!
                var userLocation = geometryFactory.CreatePoint(new Coordinate(request.Dto.longitude,request.Dto.latitiude ));
                var dto = request.Dto;

                // Check if user already exists
                var existingUser = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Email == dto.Email || u.PhoneNumber == dto.PhoneNumber);

                if (existingUser != null)
                {
                    return ResponseViewModel<DoctorDto>.FailureResponse(
                        message: "User already exists",
                        status: 409,
                        errors: new List<object> { "A user with this email or phone number already exists" }
                    );
                }

                // Generate temporary password
                var tempPassword = GenerateTemporaryPassword();

                // Create ApplicationUser
                var user = new ApplicationUser
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    ProfileImageUrl = dto.ProfileImageUrl,
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, tempPassword);

                if (!result.Succeeded)
                {
                    return ResponseViewModel<DoctorDto>.FailureResponse(
                        message: "Failed to create doctor account",
                        status: 400,
                        errors: result.Errors.Select(e => (object)e.Description).ToList()
                    );
                }

                // Assign Doctor role
                await _userManager.AddToRoleAsync(user, "Doctor");

                // Create Doctor profile
                var doctor = new Doctor
                {
                    UserId = user.Id,
                    SpecialtyId = dto.SpecialtyId,
                    LicenseNumber = dto.LicenseNumber,
                    About = dto.Bio,
                    ClinicName = dto.ClinicName,
                    SessionPrice = dto.SessionPrice,
                    Location = userLocation,

                    CreatedAt = DateTime.UtcNow
                };

                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync(cancellationToken);

                // TODO: Send credentials via email
                _logger.LogInformation($"Doctor created: {user.Email}, Temp Password: {tempPassword}");

                // Map to DTO
                var doctorDto = MapToDto(doctor, user);

                return ResponseViewModel<DoctorDto>.SuccessResponse(
                    data: doctorDto,
                    message: "Doctor created successfully. Credentials sent via email.",
                    status: 201
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating doctor");
                return ResponseViewModel<DoctorDto>.FailureResponse(
                    message: "An error occurred while creating doctor",
                    status: 500,
                    errors: new List<object> { ex.Message }
                );
            }

        }
        private string GenerateTemporaryPassword()
        {
            var random = new Random();
            var chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@#$%";
            return new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private DoctorDto MapToDto(Doctor doctor, ApplicationUser user)
        {
            return new DoctorDto
            {
                Id = doctor.Id,
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber ?? "",
                ProfileImageUrl = user.ProfileImageUrl,
                SpecialtyId = doctor.SpecialtyId,
                LicenseNumber = doctor.LicenseNumber,
                About = doctor.About,
                ClinicName = doctor.ClinicName,
                SessionPrice = doctor.SessionPrice,
                langitude = doctor.Location.X,
                latitude = doctor.Location.Y,
                
                CreatedAt = doctor.CreatedAt
            };
        }
    }
}
