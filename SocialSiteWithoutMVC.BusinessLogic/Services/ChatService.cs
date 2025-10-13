using Microsoft.EntityFrameworkCore;
using SocialSiteWithoutMVC.DataAccessLayer;
using SocialSiteWithoutMVC.DataAccessLayer.Models;

namespace SocialSiteWithoutMVC.BusinessLogic.Services;

public class ChatService(SocialSiteDbContext context)
{
    public async Task<bool> AddMessage(string text, string loginFrom, string loginTo)
    {
        var chat = await GetOrCreateChat(loginFrom, loginTo);
        if (chat == null)
            return false;
        var newMessage = new MessageEntity
        {
            Id = Guid.CreateVersion7(),
            Text = text,
            UserLogin = loginFrom
        };
        chat.Messages!.Add(newMessage);
        await context.Messages.AddAsync(newMessage);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddMessageToGroup(string text, string loginFrom, string name)
    {
        var chat = await GetGroupChat(loginFrom, name, true);
        if (chat == null)
            return false;
        var newMessage = new MessageEntity
        {
            Id = Guid.CreateVersion7(),
            Text = text,
            UserLogin = loginFrom
        };
        chat.Messages ??= new List<MessageEntity>(1);
        chat.Messages.Add(newMessage);
        await context.Messages.AddAsync(newMessage);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<ChatEntity?> GetChat(string loginFrom, string loginTo)
    {
        var chat = await context.Chats
            .AsNoTracking()
            .Where(c => c.Users.Any(u => u!.Login == loginFrom) 
                                         && c.Users.Any(u => loginTo.Contains(u!.Login)))
            .Include(c => c.Users)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync();
        return chat;
    }

    public async Task<ChatEntity?> GetGroupChat(string loginFrom, string name, bool tracking = false)
    {
        if (tracking)
        {
            return await context.Chats
                .Where(c => c.Name == name)
                .Where(c => c.Users.Any(u => u!.Login == loginFrom))
                .Include(c => c.Messages)
                .Include(c => c.Users)
                .FirstOrDefaultAsync();
        }
        return await context.Chats
            .AsNoTracking()
            .Where(c => c.Name == name)
            .Where(c => c.Users.Any(u => u!.Login == loginFrom))
            .Include(c => c.Messages)
            .Include(c => c.Users)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> CreateGroup(string loginFrom, string[] loginTo, string name)
    {
        var chat = await GetGroupChat(loginFrom, name);
        if (chat is not null)
            return false;
        var users = await context.Users
            .Where(u => u.Login == loginFrom 
                        || loginTo.Contains(u.Login))
            .Include(u => u.Chats)
            .ToArrayAsync();
        if (users.Length < 2)
            return false;
        var message = new MessageEntity
        {
            Id = Guid.CreateVersion7(),
            Text = $"{loginFrom} создал чат {name}",
            UserLogin = loginFrom
        };
        chat = new ChatEntity
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            Users = users.ToList()!,
            Messages = [ message ]
        };
        await context.Chats.AddAsync(chat);
        await context.Messages.AddAsync(message);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddUserToGroup(string loginFrom, string loginToAdd, string name)
    {
        var chat = await GetGroupChat(loginFrom, name, true);
        if (chat is null)
            return false;
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Login == loginToAdd);
        if (user is null)
            return false;
        chat.Users.Add(user);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> QuitFromGroup(string loginFrom, string name)
    {
        var chat = await GetGroupChat(loginFrom, name, true);
        if (chat is null)
            return false;   
        chat.Users.Remove(chat.Users.FirstOrDefault(u => u!.Login == loginFrom));
        if (chat.Users.Count == 0)
        {
            await context.Chats
                .Where(c => c.Name == name)
                .Where(c => c.Users.Any(u => u!.Login == loginFrom))
                .Include(c => c.Messages)
                .ExecuteDeleteAsync();
        }
        else
            await context.SaveChangesAsync();
        return true;
    }

    private async Task<ChatEntity?> GetOrCreateChat(string loginFrom, string loginTo)
    {
        var chat = await context.Chats
            .Where(c => c.Name == $"{loginFrom}_{loginTo}" 
                        || c.Name == $"{loginTo}_{loginFrom}")
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
            Id = Guid.CreateVersion7(),
            Name = $"{loginFrom}_{loginTo}",
            Messages = new List<MessageEntity>(1)
        };
        foreach (var user in users)
        {
            user.Chats ??= new List<ChatEntity>(1);
            user.Chats!.Add(newChat);
        }
        context.Chats.Add(newChat);
        return newChat;
    }
}