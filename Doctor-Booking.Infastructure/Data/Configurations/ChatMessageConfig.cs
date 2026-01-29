
using Doctor_Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctor_Booking.Infastructure.Data.Configurations;

public class ChatMessageConfig : IEntityTypeConfiguration<ChatMessage>
{
	public void Configure(EntityTypeBuilder<ChatMessage> builder)
	{
        builder.HasOne(cm => cm.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(cm => cm.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cm => cm.Sender)
            .WithMany()
            .HasForeignKey(cm => cm.SenderUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
