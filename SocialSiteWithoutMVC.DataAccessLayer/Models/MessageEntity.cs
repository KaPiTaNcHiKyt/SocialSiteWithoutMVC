namespace SocialSiteWithoutMVC.DataAccessLayer.Models;

public record MessageEntity
{
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
    public string Text { get; set; } = null!;
    public Guid ChatId { get; set; }
    public string UserLogin { get; set; } = null!;
}