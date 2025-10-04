namespace SocialSiteWithoutMVC.Models;

public class ChatModel
{
    public string[] UsersLogin { get; set; } = null!;
    public MessageModel[]? Messages { get; set; } = null!;
}