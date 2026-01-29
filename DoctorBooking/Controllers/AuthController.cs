using Doctor_Booking.Application.DTOs.Auth;
using Doctor_Booking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly IOtpService _otpService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            IOtpService otpService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _otpService = otpService;
            _logger = logger;
        }


        [HttpPost("register/patient")]
        public async Task<IActionResult> RegisterPatient([FromBody] PatientRegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterPatientAsync(dto);

            if (!result.IsSucsess)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.VerifyOtpAndActivateAsync(dto);

            if (!result.IsSucsess)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost("login/phone")]
        public async Task<IActionResult> LoginWithPhone([FromBody] LoginPhoneDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginWithPhoneAsync(dto);

            if (!result.IsSucsess)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost("login/verify-otp")]
        public async Task<IActionResult> VerifyLoginOtp([FromBody] VerifyLoginOtpDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.VerifyLoginOtpAsync(dto);

            if (!result.IsSucsess)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost("login/google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] GoogleLoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginWithGoogleAsync(dto);

            if (!result.IsSucsess)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ForgotPasswordAsync(dto);

            if (!result.IsSucsess)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ResetPasswordAsync(dto);

            if (!result.IsSucsess)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ResendOtpAsync(dto);

            if (!result.IsSucsess)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpGet("otp/remaining-time")]
        public IActionResult GetOtpRemainingTime([FromQuery] string phoneNumber, [FromQuery] string type = "registration")
        {
            var remainingTime = _otpService.GetRemainingTime(phoneNumber, type);

            if (!remainingTime.HasValue)
                return NotFound(new { message = "OTP not found or expired" });

            return Ok(new { remainingSeconds = remainingTime.Value });
        }


        [Authorize]
        [HttpGet("test")]
        public IActionResult Test()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();

            return Ok(new
            {
                message = "Authentication IsSucsessful",
                userId,
                userName,
                roles
            });
        }
    }

}
