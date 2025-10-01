using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSiteWithoutMVC.DataAccessLayer.Models;

namespace SocialSiteWithoutMVC.DataAccessLayer.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Login);

        builder
            .HasMany(u => u.Chats)
            .WithMany(c => c.Users);

        builder
            .HasMany<MessageEntity>()
            .WithOne()
            .HasForeignKey(m => m.UserLogin);
    }
}