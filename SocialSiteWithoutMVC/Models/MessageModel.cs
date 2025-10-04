namespace SocialSiteWithoutMVC.Models;

public class MessageModel
{
    public DateTime DateTime { get; set; }
    public string Text { get; set; } = null!;
    public string UserLogin { get; set; } = null!;
}