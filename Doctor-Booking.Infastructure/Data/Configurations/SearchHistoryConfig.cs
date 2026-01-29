using Doctor_Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctor_Booking.Infastructure.Data.Configurations
{
    public class SearchHistoryConfig : IEntityTypeConfiguration<SearchHistory>
    {
        public void Configure(EntityTypeBuilder<SearchHistory> builder)
        {
            builder.HasOne(s => s.Patient)
                .WithMany(p => p.SearchHistories)
                .HasForeignKey(s => s.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(sh => sh.Doctor)
                .WithMany(d => d.SearchHistories)
                .HasForeignKey(sh => sh.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
