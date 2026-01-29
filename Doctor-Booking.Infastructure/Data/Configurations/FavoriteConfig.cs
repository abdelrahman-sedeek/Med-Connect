
using Doctor_Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctor_Booking.Infastructure.Data.Configurations;

internal class FavoriteConfig : IEntityTypeConfiguration<Favorite>
{
	public void Configure(EntityTypeBuilder<Favorite> builder)
	{
        builder.HasOne(f => f.Patient)
            .WithMany(p => p.Favorites)
            .HasForeignKey(f => f.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.Doctor)
            .WithMany()
            .HasForeignKey(f => f.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
