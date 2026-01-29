using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.Interfaces.Repositories.ChatRepoInterface;
using Doctor_Booking.Domain.Entities;
using Doctor_Booking.Infastructure.Data;
using Doctor_Booking.Infastructure.Repositories;
using Doctor_Booking.Infastructure.Repositories.ChatRepo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Doctor_Booking.Infastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IChatUserRepository, ChatUserRepository>();
            services.AddScoped<IFAQRepository,FAQRepository>();
            //services.AddScoped<IDataSeeding, DataSeeding>();
            // Register DbContext with SQL Server and NetTopologySuite
            services.AddDbContext<DoctorBookingDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    x => x.UseNetTopologySuite()
                ));

            // Register repositories
            services.AddScoped<ICacheRepository, CacheRepository>();
            
            services.AddScoped<IDoctorReadRepository, DoctorReadRepository>();

            services.AddScoped<IFavoriteRepository, FavoriteRepository>();

            services.AddScoped<IPatientRepository, PatientRepository>();

            services.AddScoped<ISearchHistoryRepository, SearchHistoryRepository>();

            services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();

            services.AddScoped<IAvailabilitySlotsRepository, AvailabilitySlotsRepository>();

            // ✅ Use IdentityBuilder
            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole<int>>()
                .AddEntityFrameworkStores<DoctorBookingDbContext>();

            return services;
        }
    }
}
