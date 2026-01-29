using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace Doctor_Booking.Infastructure.Data;

public class DoctorBookingDbContext
	: IdentityDbContext<ApplicationUser, IdentityRole<int>, int>, IDoctorBookingDbContext
{
    public DoctorBookingDbContext(
           DbContextOptions<DoctorBookingDbContext> options)
           : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		//GetExecutingAssembly get all configurations and apply it

		base.OnModelCreating(modelBuilder);
	}

	public DbSet<Doctor> Doctors { get; set; }
	public DbSet<Patient> Patients { get; set; }
	public DbSet<Booking> Bookings { get; set; }
	public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }
	public DbSet<Chat> Chats { get; set; }
	public DbSet<ChatUser> ChatUsers { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
	public DbSet<Doctor_Booking.Domain.Entities.Notification> Notifications { get; set; }
	public DbSet<Payment> Payments { get; set; }
	public DbSet<PaymentMethod> PaymentMethods { get; set; }
	public DbSet<Review> Reviews { get; set; }
	public DbSet<SearchHistory> SearchHistorys { get; set; }
	public DbSet<FAQ> FAQs { get; set; }
    public DbSet<Specialty> Specialtys { get; set; }
	public DbSet<SystemLog> SystemLogs { get; set; }
	public DbSet<UserSettings> UserSettings { get ; set ; }
}
