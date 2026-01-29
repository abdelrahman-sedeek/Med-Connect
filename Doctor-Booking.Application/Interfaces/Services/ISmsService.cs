using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Interfaces.Services
{
    public interface ISmsService
    {
        Task<bool> SendOtpAsync(string phoneNumber, string otp, string purpose = "verification");
        Task<bool> SendAsync(string phoneNumber, string message);
    }
}
