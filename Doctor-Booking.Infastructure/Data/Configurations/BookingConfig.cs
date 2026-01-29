using Doctor_Booking.Domain.Entities;
using Doctor_Booking.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctor_Booking.Infastructure.Data.Configurations;

public class BookingConfig : IEntityTypeConfiguration<Booking>
{
	public void Configure(EntityTypeBuilder<Booking> builder)
	{
        builder.HasOne(b => b.Patient)
            .WithMany(p => p.Bookings)
            .HasForeignKey(b => b.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Doctor)
            .WithMany(d => d.Bookings)
            .HasForeignKey(b => b.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.AvailabilitySlot)
            .WithMany()
            .HasForeignKey(b => b.AvailabilitySlotId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
