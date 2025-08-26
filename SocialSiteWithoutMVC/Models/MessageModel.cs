namespace SocialSiteWithoutMVC.Models;

public class MessageModel
{
    public int MessageId { get; set; }
    public DateTime DateTime { get; set; }
    public string Text { get; set; }
    public ChatModel Chat { get; set; }
}