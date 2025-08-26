namespace SocialSiteWithoutMVC.Models;

public class ChatModel
{
    public int ChatId { get; set; }
    public UserModel[] Users { get; set; }
    public MessageModel[] Messages { get; set; }
}