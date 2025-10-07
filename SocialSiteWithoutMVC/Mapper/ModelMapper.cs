using Riok.Mapperly.Abstractions;
using SocialSiteWithoutMVC.DataAccessLayer.Models;
using SocialSiteWithoutMVC.Models;

namespace SocialSiteWithoutMVC.Mapper;

[Mapper]
public static partial class ModelMapper
{
    public static partial UserModel UserEntityToModel(UserEntity user);
    
    
    [UserMapping(Default = true)]
    public static ChatModel ChatEntityToModel(ChatEntity chat)
    {
        var model = ChatEntityToModelDefault(chat);
        model.UsersLogin = chat.Users
            .Select(u => u.Login)
            .ToArray();
        return model;
    }
    
    
    public static partial MessageModel MessageEntityToModel(MessageEntity message);
    
    
    [MapperIgnoreSource(nameof(UserEntity.Chats))]
    [MapperIgnoreTarget(nameof(UserModel.Chats))]
    public static partial UserModel UserEntityToModelWithoutChats(UserEntity user);
    
    
    [MapperIgnoreSource(nameof(ChatEntity.Users))]
    [MapperIgnoreTarget(nameof(ChatModel.UsersLogin))]
    [UserMapping(Ignore = true)]
    private static partial ChatModel ChatEntityToModelDefault(ChatEntity chat);
}