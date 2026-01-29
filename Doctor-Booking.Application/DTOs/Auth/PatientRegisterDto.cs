using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.DTOs.Auth
{
    public record PatientRegisterDto
    (
        string PhoneNumber,
        string Email,
        string Password,
        string ConfirmPassword,
        string FirstName,
        string LastName
    );
}
