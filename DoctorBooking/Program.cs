

using Doctor_Booking.Application.Common.Behaviours;
using Doctor_Booking.Infastructure.Notification;
using Doctor_Booking.Application.Common;
using Doctor_Booking.Application.Interfaces.Repositories;

using Doctor_Booking.Domain.Entities;
using Doctor_Booking.Infastructure.Repositories;
using DoctorBooking.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using DoctorBooking.Hubs;
using DoctorBooking.Middleware;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using Doctor_Booking.Application;
using Doctor_Booking.Infastructure;
using Doctor_Booking.Application.Interfaces.Services;
using Doctor_Booking.Application.Services;
using Doctor_Booking.Domain.Entities;
using Microsoft.OpenApi.Models;
using DoctorBooking.CurrentUser;

//pm_card_visa


var builder = WebApplication.CreateBuilder(args);

// ================= Services =================
builder.Services.AddControllers();
builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSignalR();
builder.Services.AddScoped<INotificationPublisher, SignalRNotificationPublisher>();


builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(
        builder.Configuration.GetConnectionString("RedisConnection")!));

builder.Services.AddScoped<IDoctorBookingDbContext>(
    provider => provider.GetRequiredService<DoctorBookingDbContext>());//DI for the DBContext

builder.Services.AddValidatorsFromAssembly(typeof(ValidationBehavior<,>).Assembly);


//current User Service DI
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

//payment dependency Injection
builder.Services.AddScoped<IPaymentGateway, StripePaymentGateway>();
//refund
builder.Services.AddScoped<IPaymentRefundGateway, StripePaymentGateway>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Identity
builder.Services.AddIdentityConfiguration(builder.Configuration);

// JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Memory Cache (OTP)
builder.Services.AddMemoryCache();

//Notification DI
builder.Services.AddScoped<INotificationService, NotificationService>();

// Application Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISmsService, TwilioSmsService>();

//builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IOtpService, OTPService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//using (var scope = app.Services.CreateScope())
//{
//    var seeder = scope.ServiceProvider
//        .GetRequiredService<IDataSeeding>();
//}

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DoctorBookingDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

        // Apply migrations
        //await context.Database.MigrateAsync();

        // Seed data
        await ApplicationDbContextSeed.SeedAsync(context, userManager, roleManager);
        
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.UseMiddleware<GlobalExceptionMiddleware>();

// ✅ Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Doctor Booking API V1");
        options.RoutePrefix = string.Empty; // Makes Swagger UI available at root (https://localhost:xxxx/)
        options.DocumentTitle = "Doctor Booking API";
        options.DisplayRequestDuration(); // Shows request duration
        options.EnableDeepLinking(); // Enables deep linking for operations
        options.EnableFilter(); // Enables filter box
        options.ShowExtensions(); // Shows extensions
        options.EnableValidator(); // Enables validator
    });
}
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");
app.MapHub<NotificationHub>("/hubs/notifications");


app.Run();