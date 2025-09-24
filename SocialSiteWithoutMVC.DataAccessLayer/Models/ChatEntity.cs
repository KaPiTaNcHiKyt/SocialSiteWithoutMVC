namespace SocialSiteWithoutMVC.DataAccessLayer.Models;

public record ChatEntity
{
    public Guid Id { get; set; }
    public List<UserEntity> Users { get; set; }
    public List<MessageEntity>? Messages { get; set; }
}