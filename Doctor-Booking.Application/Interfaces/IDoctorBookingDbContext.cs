using Doctor_Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Doctor_Booking.Application.Interfaces;

public interface IDoctorBookingDbContext
{
	public DbSet<ApplicationUser> Users { get; set; }
	public DbSet<Doctor> Doctors { get; set; }
	public DbSet<Patient> Patients { get; set; }
	public DbSet<Booking> Bookings { get; set; }
	public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }
	public DbSet<Chat> Chats { get; set; }
	public DbSet<ChatUser> ChatUsers { get; set; }
	public DbSet<ChatMessage> ChatMessages { get; set; }
	public DbSet<Favorite> Favorites { get; set; }
	public DbSet<Notification> Notifications { get; set; }
	public DbSet<Payment> Payments { get; set; }
	public DbSet<PaymentMethod> PaymentMethods { get; set; }
	public DbSet<Review> Reviews { get; set; }
	public DbSet<SearchHistory> SearchHistorys { get; set; }
	public DbSet<FAQ> FAQs { get; set; }
	public DbSet<Specialty> Specialtys { get; set; }
	public DbSet<SystemLog> SystemLogs { get; set; }
	DbSet<UserSettings> UserSettings { get; set; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken);

}


