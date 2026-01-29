using Doctor_Booking.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DoctorBooking.Extensions
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdentityConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
           

            services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                // Password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                // User
                options.User.RequireUniqueEmail = true;

                // Lockout
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddEntityFrameworkStores<DoctorBookingDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
