using SocialSiteWithoutMVC.DataAccessLayer.Interfaces;

namespace SocialSiteWithoutMVC.DataAccessLayer.Models;

public class ChatEntity 
    : IEntity
{
    public int ChatId { get; set; }
    public List<UserEntity> Users { get; set; }
    public List<MessageEntity>? Messages { get; set; }
}