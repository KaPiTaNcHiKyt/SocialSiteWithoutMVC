namespace SocialSiteWithoutMVC.DataAccessLayer.Models;

public record MessageEntity
{
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
    public string Text { get; set; }
    public Guid ChatId { get; set; }
    public ChatEntity Chat { get; set; }
}