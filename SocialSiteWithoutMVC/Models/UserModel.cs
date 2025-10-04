using System.ComponentModel.DataAnnotations;

namespace SocialSiteWithoutMVC.Models;

public class UserModel
{
    public string Nickname { get; set; } = null!;
    public string Login { get; set; } = null!;
    public List<ChatModel>? Chats { get; set; }
}