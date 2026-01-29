using Doctor_Booking.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.ViewModels.Auth
{
    public class AuthViewModel
    {
            public string? Token { get; set; }
            public UserDto? User { get; set; }
       
    }
}
