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
            .HasOne<ChatEntity>()
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChatId);

        builder
            .HasOne<UserEntity>()
            .WithOne()
            .HasForeignKey<MessageEntity>(m => m.UserLogin);
    }
}