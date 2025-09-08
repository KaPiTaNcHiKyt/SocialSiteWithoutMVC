using Microsoft.EntityFrameworkCore;
using SocialSiteWithoutMVC.DataAccessLayer.Interfaces;
using SocialSiteWithoutMVC.DataAccessLayer.Models;

namespace SocialSiteWithoutMVC.DataAccessLayer.Repositories;

public class ChatRepository(SocialSiteDbContext context) : IRepository
{
    public async Task AddNew(ChatEntity chat, string loginFrom, string loginTo)
    {
        var userFrom = await context.Users
            .Include(userEntity => userEntity.Chats)
            .FirstOrDefaultAsync(u => u.Login == loginFrom);
        
        var userTo = await context.Users
            .Include(userEntity => userEntity.Chats)
            .FirstOrDefaultAsync(u => u.Login == loginTo);

        AddChat(userFrom!, userTo!, chat);
        
        await context.SaveChangesAsync();
    }
    
    public async Task<bool> ChatIsInDataBase(string loginFrom, string loginTo)
    {
        return await context.Chats
            .AsNoTracking()
            .Where(c => 
                c.Users[0].Login == loginFrom && c.Users[1].Login == loginTo || 
                c.Users[0].Login == loginTo && c.Users[1].Login == loginFrom)
            .AnyAsync();
    }

    public async Task<ChatEntity?> Get(string loginFrom, string loginTo)
    {
        return await context.Chats
            .AsNoTracking()
            .Where(c => 
                c.Users[0].Login == loginFrom && c.Users[1].Login == loginTo || 
                c.Users[0].Login == loginTo && c.Users[1].Login == loginFrom)
            .Include(c => c.Messages)
            .Include(c => c.Users)
            .FirstOrDefaultAsync();
    }

    private void AddChat(UserEntity userFrom, UserEntity userTo, ChatEntity chat)
    {
        if (userFrom.Chats is not null)
            userFrom.Chats.Add(chat);
        else
            userFrom.Chats = [chat];

        if (userTo.Chats is not null)
            userTo.Chats.Add(chat);
        else
            userTo.Chats = [chat];
    }
}