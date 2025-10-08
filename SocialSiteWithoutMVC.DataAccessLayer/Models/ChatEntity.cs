namespace SocialSiteWithoutMVC.DataAccessLayer.Models;

public record ChatEntity
{
    public string Name { get; set; } = null!;
    public List<UserEntity> Users { get; set; } = null!;
    public List<MessageEntity>? Messages { get; set; }
}