
using Doctor_Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctor_Booking.Infastructure.Data.Configurations;

public class AvailSlotConfig : IEntityTypeConfiguration<AvailabilitySlot>
{
	public void Configure(EntityTypeBuilder<AvailabilitySlot> builder)
	{
        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.AvailabilitySlots)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
