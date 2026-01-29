using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Interfaces.Services
{
    public interface IOtpService
    {
        string Generate(string identifier, string type = "default");
        bool Verify(string identifier, string otp, string type = "default");
        bool Exists(string identifier, string type = "default");
        bool Invalidate(string identifier, string type = "default");
        string Resend(string identifier, string type = "default");
        int? GetRemainingTime(string identifier, string type = "default");
    }
}
