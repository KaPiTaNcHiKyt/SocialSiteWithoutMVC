using SocialSiteWithoutMVC.DataAccessLayer.Models;
using SocialSiteWithoutMVC.Models;

namespace SocialSiteWithoutMVC.Extensions;

public static class ParseEntity
{
    public static UserModel ToModel(this UserEntity user)
    {
        return new UserModel
        {
            Nickname = user.Nickname,
            Login = user.Login
        };
    }
    
    public static ChatModel ToModel(this ChatEntity chat, string thisLogin)
    {
        var chatModel = new ChatModel
        {
            UsersLogin = chat.Users
                .Where(u => u.Login != thisLogin)
                .Select(u => u.Login).ToArray(),
            Messages = chat.Messages!.Select(m => m.ToModel()).ToArray()
        };
        
        return chatModel;
    }
    
    public static ChatModel ToModelWithoutMessages(this ChatEntity chat)
    {
        var chatModel = new ChatModel
        {
            UsersLogin = chat.Users.Select(u => u.Login).ToArray()
        };
        
        return chatModel;
    }
    
    public static MessageModel ToModel(this MessageEntity message)
    {
        var messageModel = new MessageModel
        {
            DateTime = message.DateTime,
            Text = message.Text,
            UserLogin = message.UserLogin
        };
        
        return messageModel;
    }
}