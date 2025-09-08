using System.ComponentModel.DataAnnotations;

namespace SocialSiteWithoutMVC.Models;

public class UserModel
{
    public string Nickname { get; set; }
    public string Login { get; set; }
    public ChatModel[]? ChatModels { get; set; }
}