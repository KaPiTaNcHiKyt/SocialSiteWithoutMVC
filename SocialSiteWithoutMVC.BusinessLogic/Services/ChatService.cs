using SocialSiteWithoutMVC.DataAccessLayer.Models;
using SocialSiteWithoutMVC.DataAccessLayer.Repositories;

namespace SocialSiteWithoutMVC.BusinessLogic.Services;

public class ChatService(ChatRepository chatRepository, UserRepository userRepository, MessageRepository messageRepository)
{
    public async Task<bool> AddMessage(string text, string loginFrom, string loginTo)
    {
        if (!await userRepository.IsInDataBase(loginTo)) 
            return false;
        
        if (!await chatRepository.ChatIsInDataBase(loginFrom, loginTo))
            return await AddNewChat(text, loginFrom, loginTo);
        
        await AddMessageToChat(text, loginFrom, loginTo);
        return true;
    }
    
    public async Task<ChatEntity?> GetChat(string loginFrom, string loginTo) 
        => await chatRepository.Get(loginFrom, loginTo);

    private async Task<bool> AddNewChat(string text, string loginFrom, string loginTo)
    {
        var chat = new ChatEntity
        {
            Messages = 
            [
                new MessageEntity
                {
                    Text = text
                }
            ]
        };
        await chatRepository.AddNew(chat, loginFrom, loginTo);
        return true;
    }

    private async Task AddMessageToChat(string text, string loginFrom, string loginTo)
    {
        var message = new MessageEntity
        {
            Text = text
        };
        await messageRepository.AddMessage(message, loginFrom, loginTo);
    }

    public async Task<ChatEntity[]?> GetAllByLogin(string resultTestCookie)
        => await chatRepository.GetAllByLogin(resultTestCookie);
}