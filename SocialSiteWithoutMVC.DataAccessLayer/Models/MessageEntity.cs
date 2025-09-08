using SocialSiteWithoutMVC.DataAccessLayer.Interfaces;

namespace SocialSiteWithoutMVC.DataAccessLayer.Models;

public class MessageEntity
    :IEntity
{
    public int MessageId { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
    public string Text { get; set; }
    public int ChatId { get; set; }
    public ChatEntity Chat { get; set; }
}