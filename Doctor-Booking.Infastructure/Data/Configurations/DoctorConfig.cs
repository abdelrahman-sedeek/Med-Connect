using Doctor_Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctor_Booking.Infastructure.Data.Configurations;

public class DoctorConfig : IEntityTypeConfiguration<Doctor>
{
	public void Configure(EntityTypeBuilder<Doctor> builder)
	{
		builder.Property(p => p.LicenseNumber)
            .IsRequired();

        builder.Property(d => d.SessionPrice)
            .HasPrecision(18, 2);

        builder.HasOne(d => d.User)
            .WithOne(u => u.Doctor)
            .HasForeignKey<Doctor>(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Specialty)
            .WithMany(s => s.Doctors)
            .HasForeignKey(d => d.SpecialtyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Tell EF Core to store Point as 'geography' type
        builder.Property(d => d.Location)
            .HasColumnType("geography")  // Uses Earth's curvature for accuracy
            .IsRequired();

        //// Create spatial index for fast queries
        //builder.HasIndex(d => d.Location)
        //       .HasDatabaseName("IX_Doctor_Location");
    }
}
