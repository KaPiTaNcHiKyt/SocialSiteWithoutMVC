using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SocialSiteWithoutMVC.DataAccessLayer;
using SocialSiteWithoutMVC.DataAccessLayer.Models;

namespace SocialSiteWithoutMVC.BusinessLogic.Services;

public class ChatService(SocialSiteDbContext context, IMemoryCache cache)
{
    public async Task<bool> AddMessage(string text, string loginFrom, string loginTo)
    {
        var chat = await context.Chats
            .Where(c =>
                (c.Users[0].Login == loginFrom && c.Users[1].Login == loginTo) ||
                (c.Users[1].Login == loginFrom && c.Users[0].Login == loginTo))
            .Include(c => c.Messages)
            .FirstOrDefaultAsync();
        if (chat == null)
        {
            var users = await context.Users
                .Where(u => u.Login == loginTo || u.Login == loginFrom)
                .Include(u => u.Chats)
                .ToArrayAsync();
            if (users.Length < 2)
                return false;
            var newChat = new ChatEntity
            {
                Messages =
                [
                    new MessageEntity
                    {
                        Text = text,
                        Id = Guid.NewGuid(),
                        UserLogin = loginFrom
                    }
                ],
                Id = Guid.NewGuid()
            };
            foreach (var user in users)
            {
                user.Chats ??= new List<ChatEntity>(1);
                user.Chats!.Add(newChat);
            }
            context.Chats.Add(newChat);
        }
        else
        {
            var newMessage = new MessageEntity
            {
                Id = Guid.NewGuid(),
                Text = text,
                UserLogin = loginFrom
            };
            chat.Messages!.Add(newMessage);
            context.Messages.Add(newMessage);
        }
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<ChatEntity?> GetChat(string loginFrom, string loginTo)
    {
        if (cache.TryGetValue($"{loginFrom}{loginTo}", out ChatEntity? chat))
            return chat;
        chat = await context.Chats
            .AsNoTracking()
            .Where(c =>
                (c.Users[0].Login == loginFrom && c.Users[1].Login == loginTo) ||
                (c.Users[1].Login == loginFrom && c.Users[0].Login == loginTo))
            .Include(c => c.Users)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync();
        if (chat != null)
        {
            cache.Set($"{loginFrom}_{loginTo}", chat, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(2)));
        }
        return chat;
    }

    public async Task<ChatEntity[]?> GetAllByLogin(string myLogin)
    {
        if (cache.TryGetValue($"{myLogin}", out ChatEntity[]? chats))
            return chats;
        chats = await context.Chats
            .AsNoTracking()
            .Include(c => c.Users)
            .Where(c => c.Users.Any(u => u.Login == myLogin))
            .ToArrayAsync();
        if (chats.Length == 0)
        {
            cache.Set($"{myLogin}", chats, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
        }
        return chats;
    }
}