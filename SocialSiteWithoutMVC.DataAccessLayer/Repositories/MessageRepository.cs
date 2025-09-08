using Microsoft.EntityFrameworkCore;
using SocialSiteWithoutMVC.DataAccessLayer.Interfaces;
using SocialSiteWithoutMVC.DataAccessLayer.Models;

namespace SocialSiteWithoutMVC.DataAccessLayer.Repositories;

public class MessageRepository(SocialSiteDbContext context) : IRepository
{
    public async Task AddMessage(MessageEntity message, string loginFrom, string loginTo)
    {
        var chat = await context.Chats
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => 
                c.Users[0].Login == loginFrom && c.Users[1].Login == loginTo || 
                c.Users[0].Login == loginTo && c.Users[1].Login == loginFrom);
        
        chat!.Messages!.Add(message);
        
        await context.SaveChangesAsync();
    }
}