using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSiteWithoutMVC.DataAccessLayer.Models;

namespace SocialSiteWithoutMVC.DataAccessLayer.Configurations;


public class MessageConfiguration : IEntityTypeConfiguration<MessageEntity>
{
    public void Configure(EntityTypeBuilder<MessageEntity> builder)
    {
        builder.HasKey(m => m.Id);

        builder
            .HasOne(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChatId);

        builder
            .HasOne(m => m.User)
            .WithOne()
            .HasForeignKey<UserEntity>(u => u.Login);
    }
}