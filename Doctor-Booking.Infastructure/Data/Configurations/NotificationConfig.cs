using Doctor_Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctor_Booking.Infastructure.Data.Configurations
{
    public class NotificationConfig : IEntityTypeConfiguration<Doctor_Booking.Domain.Entities.Notification>
    {
		public void Configure(EntityTypeBuilder< Doctor_Booking.Domain.Entities.Notification> builder)
        {
            builder.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
