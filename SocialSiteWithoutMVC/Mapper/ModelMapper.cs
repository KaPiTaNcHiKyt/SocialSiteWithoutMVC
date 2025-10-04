using Riok.Mapperly.Abstractions;
using SocialSiteWithoutMVC.DataAccessLayer.Models;
using SocialSiteWithoutMVC.Models;

namespace SocialSiteWithoutMVC.Mapper;

[Mapper]
public static partial class ModelMapper
{
    [MapperIgnoreSource(nameof(UserEntity.Password))]
    [MapperIgnoreSource(nameof(UserEntity.Chats))]
    [MapperIgnoreTarget(nameof(UserModel.Chats))]
    public static partial UserModel UserEntityToModel(UserEntity user);
    
    [MapperIgnoreSource(nameof(ChatEntity.Id))]
    [MapperIgnoreSource(nameof(ChatEntity.Users))]
    [MapperIgnoreTarget(nameof(ChatModel.UsersLogin))]
    public static partial ChatModel ChatEntityToModel(ChatEntity chat);
    
    [MapperIgnoreSource(nameof(MessageEntity.Id))]
    [MapperIgnoreSource(nameof(MessageEntity.ChatId))]
    public static partial MessageModel MessageEntityToModel(MessageEntity message);

    public static ChatModel ChatEntityToModel(ChatEntity chat, string thisLogin)
    {
        var model = ChatEntityToModel(chat);
        model.UsersLogin = chat.Users
            .Where(u => u.Login != thisLogin)
            .Select(u => u.Login).ToArray();
        return model;
    }

    public static ChatModel ChatEntityToModelWithoutMessages(ChatEntity chat, string thisLogin)
    {
        var model = ChatEntityToModel(chat, thisLogin);
        model.Messages = null;
        return model;
    }
}