namespace SocialSiteWithoutMVC.DataAccessLayer.Models;

public record UserEntity
{
    public string Nickname { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public List<ChatEntity>? Chats { get; set; }
};