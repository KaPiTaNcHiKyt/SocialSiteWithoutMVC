using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SocialSiteWithoutMVC.DataAccessLayer;
using SocialSiteWithoutMVC.DataAccessLayer.Models;

namespace SocialSiteWithoutMVC.BusinessLogic.Services;

public class ChatService(SocialSiteDbContext context)
{
    public async Task<bool> AddMessage(string text, string loginFrom, string[] loginTo, string name = "") // string.Empty
    {
        var chat = await GetOrCreateChat(loginFrom, loginTo, name);
        if (chat == null)
            return false;
        var newMessage = new MessageEntity
        {
            Id = Guid.NewGuid(),
            Text = text,
            UserLogin = loginFrom
        };
        chat.Messages!.Add(newMessage);
        context.Messages.Add(newMessage);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<ChatEntity?> GetChat(string loginFrom, string[] loginTo)
    {
        var chatName = $"{loginFrom}_{loginTo.Aggregate(string.Empty,
            (curr, next) => curr + $"{next} ")}";
        var chat = await context.Chats
            .AsNoTracking()
            // .Where(c => c.Users.Any(u => u.Login == loginFrom 
            //                              && loginTo.Contains(u.Login)))
            .Where(c => c.Name == chatName)
            .Include(c => c.Users)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync();
        return chat;
    }

    private async Task<ChatEntity?> GetOrCreateChat(string loginFrom, string[] loginTo, string name)
    {
        var chat = await context.Chats
            .Where(c => c.Users.Any(u => u.Login == loginFrom 
                                         && loginTo.Contains(u.Login)))
            .Include(c => c.Messages)
            .FirstOrDefaultAsync();
        if (chat != null)
            return chat;
        var users = await context.Users
            .Where(u => u.Login == loginFrom 
                        || loginTo.Contains(u.Login))
            .Include(u => u.Chats)
            .ToArrayAsync();
        if (users.Length < 2)
            return null;
        var newChat = new ChatEntity
        {
            Messages = new List<MessageEntity>(1)
        };
        if (name == string.Empty)
        {
            newChat.Name = $"{loginFrom}_{loginTo
                .Aggregate(string.Empty, (current, next) =>
                    current + $"{next} ")}";
        }
        else
            newChat.Name = name;
        foreach (var user in users)
        {
            user.Chats ??= new List<ChatEntity>(1);
            user.Chats!.Add(newChat);
        }
        context.Chats.Add(newChat);
        return newChat;
    }
}