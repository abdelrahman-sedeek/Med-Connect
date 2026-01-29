using Doctor_Booking.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Doctor_Booking.Application.Services
{
    public class TwilioSmsService : ISmsService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TwilioSmsService> _logger;
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;

        public TwilioSmsService(
            IConfiguration configuration,
            ILogger<TwilioSmsService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            _accountSid = _configuration["Twilio:AccountSid"]
                ?? throw new ArgumentNullException("Twilio:AccountSid not configured");
            _authToken = _configuration["Twilio:AuthToken"]
                ?? throw new ArgumentNullException("Twilio:AuthToken not configured");
            _fromPhoneNumber = _configuration["Twilio:PhoneNumber"]
                ?? throw new ArgumentNullException("Twilio:PhoneNumber not configured");

           TwilioClient.Init(_accountSid, _authToken);
        }
        public async Task<bool> SendOtpAsync(string phoneNumber, string otp, string purpose = "verification")
        {
            try
            {
                var message = purpose.ToLower() switch
                {
                    "registration" => $"Welcome to Doctor Booking! Your verification code is: {otp}. Valid for 30 seconds.",
                    "login" => $"Your Doctor Booking login code is: {otp}. Valid for 30 seconds.",
                    "password_reset" => $"Your password reset code is: {otp}. Valid for 30 seconds.",
                    "2fa" => $"Your two-factor authentication code is: {otp}. Valid for 30 seconds.",
                    _ => $"Your verification code is: {otp}. Valid for 30 seconds."
                };

                return await SendAsync(phoneNumber, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending OTP to {phoneNumber}");
                return false;
            }
        }
        public async Task<bool> SendAsync(string phoneNumber, string message)
        {
            try
            {
                if (!phoneNumber.StartsWith("+"))
                {
                    _logger.LogWarning($"Phone number {phoneNumber} doesn't start with '+'. Adding country code might be needed.");
                }

                var messageResource = await MessageResource.CreateAsync(
                    body: message,
                    from: new PhoneNumber(_fromPhoneNumber),
                    to: new PhoneNumber(phoneNumber)
                );

                if (messageResource.Status == MessageResource.StatusEnum.Failed ||
                    messageResource.Status == MessageResource.StatusEnum.Undelivered)
                {
                    _logger.LogError($"Failed to send SMS to {phoneNumber}. Status: {messageResource.Status}");
                    return false;
                }

                _logger.LogInformation($"SMS sent successfully to {phoneNumber}. SID: {messageResource.Sid}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending SMS to {phoneNumber}: {ex.Message}");
                return false;
            }
        }
    }
}
