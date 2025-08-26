using SocialSiteWithoutMVC.DataAccessLayer.Models;
using SocialSiteWithoutMVC.Models;

namespace SocialSiteWithoutMVC.Extensions;

public static class ParseEntity
{
    public static UserModel ToModel(this UserEntity user)
    {
        return new UserModel
        {
            Nickname = user.NickName,
            Login = user.Login,
            ChatsId = user.Chats == null ? null 
                : (from chat in user.Chats select chat.ChatId).ToArray()
        };
    }
    
    public static ChatModel ToModel(this ChatEntity chat)
    {
        var chatModel = new ChatModel
        {
            ChatId = chat.ChatId
        };
        return chatModel;
    }
    
    public static MessageModel ToModel(this MessageEntity message)
    {
        var messageModel = new MessageModel
        {
        };
        return messageModel;
    }
}