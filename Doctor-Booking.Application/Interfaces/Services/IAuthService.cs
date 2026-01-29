using Doctor_Booking.Application.DTOs.Auth;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Application.ViewModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ResponseViewModel<AuthViewModel>> RegisterPatientAsync(PatientRegisterDto dto);
        Task<ResponseViewModel<AuthViewModel>> VerifyOtpAndActivateAsync(VerifyOtpDto dto);
        Task<ResponseViewModel<AuthViewModel>> LoginWithPhoneAsync(LoginPhoneDto dto);
        Task<ResponseViewModel<AuthViewModel>> VerifyLoginOtpAsync(VerifyLoginOtpDto dto);
        Task<ResponseViewModel<AuthViewModel>> LoginWithGoogleAsync(GoogleLoginDto dto);
        Task<ResponseViewModel<AuthViewModel>> ForgotPasswordAsync(ForgotPasswordDto dto);
        Task<ResponseViewModel<AuthViewModel>> ResetPasswordAsync(ResetPasswordDto dto);
        Task<ResponseViewModel<AuthViewModel>> ResendOtpAsync(ResendOtpDto dto);
    }
}
