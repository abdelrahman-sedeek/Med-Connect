using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.DTOs.Auth
{
    public record ResetPasswordDto(
         string PhoneNumber,
         string Otp,
         string NewPassword,
         string ConfirmPassword
     );
}
