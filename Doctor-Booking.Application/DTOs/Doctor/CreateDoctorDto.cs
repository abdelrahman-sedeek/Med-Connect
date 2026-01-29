using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.DTOs.Doctor
{
    public class CreateDoctorDto
    {
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int SpecialtyId { get; set; }
        public string LicenseNumber { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? ClinicName { get; set; }
        public decimal SessionPrice { get; set; }

        public double longitude { get; set; }
        public double latitiude  { get; set; }


        public string? ProfileImageUrl { get; set; }
    }
}
