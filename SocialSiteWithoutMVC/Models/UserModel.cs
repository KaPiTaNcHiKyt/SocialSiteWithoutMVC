using System.ComponentModel.DataAnnotations;

namespace SocialSiteWithoutMVC.Models;

public class UserModel
{
    [Required]
    public string Nickname { get; set; }
    [Required]
    public string Login { get; set; }

    public int[]? ChatsId { get; set; }
}