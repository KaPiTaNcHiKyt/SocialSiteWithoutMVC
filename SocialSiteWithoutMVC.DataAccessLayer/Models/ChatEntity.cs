namespace SocialSiteWithoutMVC.DataAccessLayer.Models;

public record ChatEntity
{
    public Ulid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<UserEntity> Users { get; set; } = null!;
    public List<MessageEntity>? Messages { get; set; }
}