using Doctor_Booking.Domain.Entities;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.DTOs.Doctor
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public int? SpecialtyId { get; set; }
        public string LicenseNumber { get; set; } = string.Empty;
        public string? About { get; set; }
        public string? ClinicName { get; set; }
        public decimal SessionPrice { get; set; }
        public double latitude { get; set; }
        public double langitude { get; set; }
        public bool IsActive { get; set; } = false;



        public DateTime CreatedAt { get; set; }
        public List<DoctorAvailabilityDto> Availabilities { get; set; } = new();

    }
}
