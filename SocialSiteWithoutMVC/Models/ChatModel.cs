namespace SocialSiteWithoutMVC.Models;

public class ChatModel
{
    public string? Name { get; set; }
    public string[] UsersLogin { get; set; } = null!;
    public MessageModel[]? Messages { get; set; } = null!;
}