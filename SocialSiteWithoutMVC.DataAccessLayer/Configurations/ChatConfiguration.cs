using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSiteWithoutMVC.DataAccessLayer.Models;

namespace SocialSiteWithoutMVC.DataAccessLayer.Configurations;

public class ChatConfiguration : IEntityTypeConfiguration<ChatEntity>
{
    public void Configure(EntityTypeBuilder<ChatEntity> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Name).IsUnique(false);

        builder
            .HasMany(c => c.Users)
            .WithMany(u => u.Chats);

        builder
            .HasMany(c => c.Messages)
            .WithOne()
            .HasForeignKey(m => m.ChatId);
    }
}