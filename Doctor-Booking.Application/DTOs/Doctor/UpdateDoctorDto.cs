using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.DTOs.Doctor
{
    public class UpdateDoctorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int SpecialtyId { get; set; } 
        public string? About { get; set; }
        public string? ClinicName { get; set; }
        public decimal SessionPrice { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool IsAvailable { get; set; }
    }
}
