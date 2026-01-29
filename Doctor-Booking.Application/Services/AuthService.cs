using Doctor_Booking.Application.DTOs.Auth;
using Doctor_Booking.Application.Interfaces.Services;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Application.ViewModels.Auth;
using Doctor_Booking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Services
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOtpService _otpService;
        private readonly IJwtService _jwtService;
        private readonly ISmsService _smsService; // Added

        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IOtpService otpService,
            IJwtService jwtService,
            ILogger<AuthService> logger,
            ISmsService smsService
            )
            
        {
            _smsService= smsService;
            _userManager = userManager;
            _otpService = otpService;
            _jwtService = jwtService;
            _logger = logger;
        }
        public async Task<ResponseViewModel<AuthViewModel>> RegisterPatientAsync(PatientRegisterDto dto)
        {
            try
            {
                var errors = new List<object>();
                var data = new AuthViewModel();
                
                // Validate passwords match
                if (dto.Password != dto.ConfirmPassword)
                {
                    return ResponseViewModel<AuthViewModel>.FailureResponse(
                          message: "Passwords do not match",
                          
                          errors: new List<object> { "Password and ConfirmPassword are not equal" }
                      );

                }

                // Check if user exists by phone or email
                var existingUser =  _userManager.Users
                    .FirstOrDefault(u => u.PhoneNumber == dto.PhoneNumber || u.Email == dto.Email);

                if (existingUser != null)
                {
                    return ResponseViewModel<AuthViewModel>.FailureResponse(
                       message: "User already exists",
                       status: 409,
                       errors: new List<object>
                       {
                            "User with this phone number or email already exists"
                       }
                   );
                }

                // Create new user
                var user = new ApplicationUser
                {
                    PhoneNumber = dto.PhoneNumber,
                    UserName = dto.PhoneNumber, // Use phone as username
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    IsActive = false, // Will be activated after OTP verification
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, dto.Password);

                if (!result.Succeeded)
                {
                    return ResponseViewModel<AuthViewModel>.FailureResponse(
                         message: "Failed to create user",
                        status: 400,
                        errors: result.Errors
                            .Select(e => (object)e.Description)
                            .ToList()
                    );
                }

                // Assign Patient role
                await _userManager.AddToRoleAsync(user, "Patient");

                // Generate OTP
                var otp = _otpService.Generate(dto.PhoneNumber, "registration");

                // TODO: Send OTP via SMS (integrate Twilio or similar)
                _logger.LogInformation($"Registration OTP for {dto.PhoneNumber}: {otp}");
                
            
                var authViewModel = new AuthViewModel
                {
                    User = new UserDto
                    {
                        Id = user.Id,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    }
                };
               
                // Send OTP via SMS
                var smsSent = await _smsService.SendOtpAsync(dto.PhoneNumber, otp, "registration");

                if (!smsSent)
                {
                    _logger.LogWarning($"Failed to send SMS to {dto.PhoneNumber}, but registration succeeded");
                }
                return ResponseViewModel<AuthViewModel>.SuccessResponse(
                  data: authViewModel,
                  status: 201,
                  message: "Registration successful. OTP sent to your phone number."
              );

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during patient registration");
                return ResponseViewModel<AuthViewModel>.FailureResponse(
                     message: "An unexpected error occurred during registration",
                     status: 500,
                     errors: new List<object> { ex.Message }
                 );
            }
        }
        public async Task<ResponseViewModel<AuthViewModel>> VerifyOtpAndActivateAsync(VerifyOtpDto dto)
        {
            try
            {
                // Verify OTP
                var isValidOtp = _otpService.Verify(dto.PhoneNumber, dto.Otp, dto.Type);

                if (!isValidOtp)
                {
                    return ResponseViewModel<AuthViewModel>.FailureResponse(
                        message: "Invalid or expired OTP",
                        status: 400,
                        errors: new List<object> { "The OTP you entered is incorrect or has expired" }
                    );
                }

                // Find user by phone number
                var user =  _userManager.Users
                    .FirstOrDefault(u => u.PhoneNumber == dto.PhoneNumber);

                if (user == null)
                {
                    return ResponseViewModel<AuthViewModel>.FailureResponse(
                        message: "User not found",
                        status: 404,
                        errors: new List<object> { "No user found with this phone number" }
                    );
                }

                // Activate user
                user.IsActive = true;
                user.PhoneNumberConfirmed = true;
                await _userManager.UpdateAsync(user);

                // Generate JWT token
                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtService.GenerateToken(user, roles);

                var AuthViewModel = new AuthViewModel
                {
                    Token = token,
                    User = MapToUserDto(user, roles)
                };

                return ResponseViewModel<AuthViewModel>.SuccessResponse(
                    data: AuthViewModel,
                    message: "Account activated successfully",
                    status: 200
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during OTP verification");
                return ResponseViewModel<AuthViewModel>.FailureResponse(
                    message: "An error occurred during verification",
                    status: 500,
                    errors: new List<object> { ex.Message }
                );
            }
        }
        public async Task<ResponseViewModel<AuthViewModel>> LoginWithPhoneAsync(LoginPhoneDto dto)
        {
            try
            {
                // Find user by phone number
                var user =  _userManager.Users
                    .FirstOrDefault(u => u.PhoneNumber == dto.PhoneNumber);

                if (user == null)
                {
                    return ResponseViewModel<AuthViewModel>.FailureResponse(
                        message: "User not found",
                        status: 404,
                        errors: new List<object> { "No account found with this phone number" }
                    );
                }
                // Generate JWT token
                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtService.GenerateToken(user, roles);

                var AuthViewModel = new AuthViewModel
                {
                    Token = token,
                    User = MapToUserDto(user, roles)
                };

                if (!user.IsActive)
                {

                    // Generate OTP for login
                    var otp = _otpService.Generate(dto.PhoneNumber, "login");

                    // TODO: Send OTP via SMS
                    _logger.LogInformation($"Login OTP for {dto.PhoneNumber}: {otp}");
                    // Send OTP via SMS
                    var smsSent = await _smsService.SendOtpAsync(dto.PhoneNumber, otp, "login");

                    if (!smsSent)
                    {
                        _logger.LogError($"Failed to send login OTP to {dto.PhoneNumber}");
                        return ResponseViewModel<AuthViewModel>.FailureResponse(
                            message: "Failed to send OTP. Please try again.",
                            status: 500
                        );
                    }
                    return ResponseViewModel<AuthViewModel>.SuccessResponse(
                        message: "OTP sent to your phone number",
                        status: 200
                    );
                }

                return ResponseViewModel<AuthViewModel>.SuccessResponse(
                 data: AuthViewModel,
                 message: "Login successful",
                 status: 200
             );


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return ResponseViewModel<AuthViewModel>.FailureResponse(
                    message: "An error occurred during login",
                    status: 500,
                    errors: new List<object> { ex.Message }
                );
            }
        }
        public async Task<ResponseViewModel<AuthViewModel>> VerifyLoginOtpAsync(VerifyLoginOtpDto dto)
        {
            try
            {
                // Verify OTP
                var isValidOtp = _otpService.Verify(dto.PhoneNumber, dto.Otp, "login");

                if (!isValidOtp)
                {
                    return ResponseViewModel<AuthViewModel>.FailureResponse(
                        message: "Invalid or expired OTP",
                        status: 400,
                        errors: new List<object> { "The OTP you entered is incorrect or has expired" }
                    );
                }

                // Find user
                var user =  _userManager.Users
                    .FirstOrDefault(u => u.PhoneNumber == dto.PhoneNumber);

                if (user == null || !user.IsActive)
                {
                    return ResponseViewModel<AuthViewModel>.FailureResponse(
                        message: "Authentication failed",
                        status: 401,
                        errors: new List<object> { "Invalid credentials or inactive account" }
                    );
                }

                // Generate JWT token
                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtService.GenerateToken(user, roles);

                var AuthViewModel = new AuthViewModel
                {
                    Token = token,
                    User = MapToUserDto(user, roles)
                };

                return ResponseViewModel<AuthViewModel>.SuccessResponse(
                    data: AuthViewModel,
                    message: "Login successful",
                    status: 200
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login OTP verification");
                return ResponseViewModel<AuthViewModel>.FailureResponse(
                    message: "An error occurred during login",
                    status: 500,
                    errors: new List<object> { ex.Message }
                );
            }
        }
        public async Task<ResponseViewModel<AuthViewModel>> LoginWithGoogleAsync(GoogleLoginDto dto)
        {
            // TODO: Implement Google authentication
            // 1. Verify Google ID token
            // 2. Extract user info (email, name, etc.)
            // 3. Find or create user
            // 4. Generate JWT token
            return ResponseViewModel<AuthViewModel>.FailureResponse(
                message: "Google login not implemented yet",
                status: 501
            );
        }
        public async Task<ResponseViewModel<AuthViewModel>> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            try
            {
                var user =  _userManager.Users
                    .FirstOrDefault(u => u.PhoneNumber == dto.PhoneNumber);

                if (user == null)
                {
                    // Don't reveal that user doesn't exist for security
                    return ResponseViewModel<AuthViewModel>.SuccessResponse(
                        message: "If the phone number exists, an OTP has been sent",
                        status: 200
                    );
                }

                // Generate OTP
                var otp = _otpService.Generate(dto.PhoneNumber, "password_reset");

                // TODO: Send OTP via SMS
                _logger.LogInformation($"Password reset OTP for {dto.PhoneNumber}: {otp}");

                return ResponseViewModel<AuthViewModel>.SuccessResponse(
                    message: "OTP sent to your phone number",
                    status: 200
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during forgot password");
                return ResponseViewModel<AuthViewModel>.FailureResponse(
                    message: "An error occurred",
                    status: 500,
                    errors: new List<object> { ex.Message }
                );
            }
        }
        public async Task<ResponseViewModel<AuthViewModel>> ResetPasswordAsync(ResetPasswordDto dto)
        {
            try
            {
                if (dto.NewPassword != dto.ConfirmPassword)
                {
                    return ResponseViewModel<AuthViewModel>.FailureResponse(
                        message: "Passwords do not match",
                        status: 400,
                        errors: new List<object> { "NewPassword and ConfirmPassword must be identical" }
                    );
                }

                // Verify OTP
                var isValidOtp = _otpService.Verify(dto.PhoneNumber, dto.Otp, "password_reset");

                if (!isValidOtp)
                {
                    return ResponseViewModel<AuthViewModel>.FailureResponse(
                        message: "Invalid or expired OTP",
                        status: 400,
                        errors: new List<object> { "The OTP you entered is incorrect or has expired" }
                    );
                }

                var user =  _userManager.Users
                    .FirstOrDefault(u => u.PhoneNumber == dto.PhoneNumber);

                if (user == null)
                {
                    return ResponseViewModel<AuthViewModel>.FailureResponse(
                        message: "User not found",
                        status: 404,
                        errors: new List<object> { "No user found with this phone number" }
                    );
                }

                // Reset password
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);

                if (!result.Succeeded)
                {
                    return ResponseViewModel<AuthViewModel>.FailureResponse(
                        message: "Password reset failed",
                        status: 400,
                        errors: result.Errors.Select(e => (object)e.Description).ToList()
                    );
                }

                return ResponseViewModel<AuthViewModel>.SuccessResponse(
                    message: "Password reset successfully",
                    status: 200
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password reset");
                return ResponseViewModel<AuthViewModel>.FailureResponse(
                    message: "An error occurred",
                    status: 500,
                    errors: new List<object> { ex.Message }
                );
            }
        }
        public async Task<ResponseViewModel<AuthViewModel>> ResendOtpAsync(ResendOtpDto dto)
        {
            try
            {
                var user =  _userManager.Users
                    .FirstOrDefault(u => u.PhoneNumber == dto.PhoneNumber);

                if (user == null)
                {
                    return ResponseViewModel<AuthViewModel>.FailureResponse(
                        message: "User not found",
                        status: 404,
                        errors: new List<object> { "No user found with this phone number" }
                    );
                }

                // Resend OTP
                var otp = _otpService.Resend(dto.PhoneNumber, dto.Type);

                // TODO: Send OTP via SMS
                _logger.LogInformation($"Resent OTP for {dto.PhoneNumber}: {otp}");

                return ResponseViewModel<AuthViewModel>.SuccessResponse(
                    message: "OTP resent successfully",
                    status: 200
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during OTP resend");
                return ResponseViewModel<AuthViewModel>.FailureResponse(
                    message: "An error occurred",
                    status: 500,
                    errors: new List<object> { ex.Message }
                );
            }
        }
        private UserDto MapToUserDto(ApplicationUser user, IList<string> roles)
        {
            return new UserDto
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber ?? "",
                Email = user.Email ?? "",
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImageUrl = user.ProfileImageUrl,
                IsActive = user.IsActive,
                Roles = roles.ToList()
            };
        }
    }
}
