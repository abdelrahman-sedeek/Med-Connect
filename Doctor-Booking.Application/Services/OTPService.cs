using Doctor_Booking.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Services
{
    public class OTPService : IOtpService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<OTPService> _logger;
        private const int OTP_EXPIRATION =120;
        private const int OTP_LENGTH = 4;
        private const string CACHE_PREFIX = "otp:";

        public OTPService(IMemoryCache cache, ILogger<OTPService> logger)
        {
            _cache = cache;
            _logger = logger;
        }
        public string Generate(string identifier, string type = "default")
        {
            var otp = GenerateOtp();
            var cacheKey = GetCacheKey(identifier, type);

            var cacheEntry = new OtpCacheEntry
            {
                Otp = otp,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddSeconds(OTP_EXPIRATION)
            };

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(OTP_EXPIRATION));

            _cache.Set(cacheKey, cacheEntry, cacheOptions);
            _logger.LogInformation($"OTP generated for {identifier} (type: {type})");

            return otp;
        }
        public bool Verify(string identifier, string otp, string type = "default")
        {
            var cacheKey = GetCacheKey(identifier, type);

            if (!_cache.TryGetValue(cacheKey, out OtpCacheEntry? storedEntry) || storedEntry == null)
            {
                _logger.LogWarning($"OTP not found or expired for {identifier} (type: {type})");
                return false;
            }

            if (storedEntry.Otp != otp)
            {
                _logger.LogWarning($"Invalid OTP attempt for {identifier} (type: {type})");
                return false;
            }

            _cache.Remove(cacheKey);
            _logger.LogInformation($"OTP verified successfully for {identifier} (type: {type})");
            return true;
        }
        public bool Exists(string identifier, string type = "default")
        {
            var cacheKey = GetCacheKey(identifier, type);
            return _cache.TryGetValue(cacheKey, out _);
        }
        public int? GetRemainingTime(string identifier, string type = "default")
        {
            var cacheKey = GetCacheKey(identifier, type);

            if (!_cache.TryGetValue(cacheKey, out OtpCacheEntry? storedEntry) || storedEntry == null)
                return null;

            var remainingSeconds = (int)(storedEntry.ExpiresAt - DateTime.UtcNow).TotalSeconds;
            return remainingSeconds > 0 ? remainingSeconds : 0;
        }
        public bool Invalidate(string identifier, string type = "default")
        {
            var cacheKey = GetCacheKey(identifier, type);
            _cache.Remove(cacheKey);
            return true;
        }
        public string Resend(string identifier, string type = "default")
        {
            Invalidate(identifier, type);
            return Generate(identifier, type);
        }

        private string GenerateOtp()
        {
            var random = new Random();
            return random.Next(0, 999999).ToString($"D{OTP_LENGTH}");
        }
        private string GetCacheKey(string identifier, string type)
        {
            return $"{CACHE_PREFIX}{type}:{identifier}";
        }

        private class OtpCacheEntry
        {
            public string Otp { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
            public DateTime ExpiresAt { get; set; }
        }
    }
}
