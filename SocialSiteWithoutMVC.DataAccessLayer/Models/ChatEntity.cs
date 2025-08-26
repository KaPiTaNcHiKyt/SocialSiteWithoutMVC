using SocialSiteWithoutMVC.DataAccessLayer.Interfaces;

namespace SocialSiteWithoutMVC.DataAccessLayer.Models;

public class ChatEntity 
    : IEntity
{
    public int ChatId { get; set; }
    public UserEntity[] Users { get; set; }
    public MessageEntity[] Messages { get; set; }
}