using Microsoft.EntityFrameworkCore;
using SocialSiteWithoutMVC.DataAccessLayer.Configurations;
using SocialSiteWithoutMVC.DataAccessLayer.Models;

namespace SocialSiteWithoutMVC.DataAccessLayer;

public class SocialSiteDbContext(DbContextOptions<SocialSiteDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<MessageEntity> Messages { get; set; }
    public DbSet<ChatEntity> Chats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());
        modelBuilder.ApplyConfiguration(new ChatConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}