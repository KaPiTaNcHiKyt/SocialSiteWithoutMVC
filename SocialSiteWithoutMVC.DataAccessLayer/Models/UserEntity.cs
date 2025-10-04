namespace SocialSiteWithoutMVC.DataAccessLayer.Models;

public record UserEntity
{
    public string Nickname { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public List<ChatEntity>? Chats { get; set; }
};