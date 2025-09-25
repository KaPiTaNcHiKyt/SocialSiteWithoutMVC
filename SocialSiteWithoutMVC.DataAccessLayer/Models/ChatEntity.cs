namespace SocialSiteWithoutMVC.DataAccessLayer.Models;

public record ChatEntity named chats feature
{
    public Guid Id { get; set; }
    public List<UserEntity> Users { get; set; }
    public List<MessageEntity>? Messages { get; set; }
}