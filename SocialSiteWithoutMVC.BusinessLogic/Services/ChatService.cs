using Microsoft.EntityFrameworkCore;
using SocialSiteWithoutMVC.DataAccessLayer;
using SocialSiteWithoutMVC.DataAccessLayer.Models;

namespace SocialSiteWithoutMVC.BusinessLogic.Services;

public class ChatService(SocialSiteDbContext context)
{
    public async Task<bool> AddMessage(string text, string loginFrom, string loginTo)
    {
        var chat = await context.Chats
            .Where(c =>
                (c.Users[0].Login == loginFrom && c.Users[1].Login == loginTo) ||
                (c.Users[1].Login == loginFrom && c.Users[0].Login == loginTo))
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
            chat.Messages!.Add(new MessageEntity
            {
                Id = Guid.NewGuid(),
                Text = text,
                UserLogin = loginFrom
            });
        }
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<ChatEntity?> GetChat(string loginFrom, string loginTo)
    {
        return await context.Chats
            .AsNoTracking()
            .Where(c =>
                (c.Users[0].Login == loginFrom && c.Users[1].Login == loginTo) ||
                (c.Users[1].Login == loginFrom && c.Users[0].Login == loginTo))
            .Include(c => c.Users)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync();
    }

    public async Task<ChatEntity[]?> GetAllByLogin(string myLogin)
    {
        return await context.Chats
            .AsNoTracking()
            .Include(c => c.Users)
            .Where(c => c.Users.Any(u => u.Login == myLogin))
            .ToArrayAsync();
    }
}