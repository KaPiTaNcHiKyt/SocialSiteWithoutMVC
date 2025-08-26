using Microsoft.EntityFrameworkCore;
using SocialSiteWithoutMVC.DataAccessLayer.Interfaces;
using SocialSiteWithoutMVC.DataAccessLayer.Models;

namespace SocialSiteWithoutMVC.DataAccessLayer.Repositories;

public class UserRepository(SocialSiteDbContext context) : SocialSiteRepository(context)
{
    public override async Task<bool> AddNew(IEntity userEntity)
    {
        var user = userEntity as UserEntity ?? throw new ArgumentException(nameof(userEntity));
        if (await IsInDataBaseByLogin(user.Login)) return false;
        
        
        await context.Users
                .AddAsync(userEntity as UserEntity ?? throw new InvalidOperationException());
        
        await context.SaveChangesAsync();
        return true;
    }

    public override async Task UpdateByPrimaryKey(IEntity entity)
    {
        await context.Users
            .Where(u => u.Login == (entity as UserEntity)!.Login)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Login, (entity as UserEntity)!.Login)
                .SetProperty(u => u.NickName, (entity as UserEntity)!.NickName)
                .SetProperty(u => u.Password, (entity as UserEntity)!.Password)
                .SetProperty(u => u.Chats, (entity as UserEntity)!.Chats));
    }

    public override async Task DeleteByPrimaryKey(string primaryKey)
    {
        await context.Users
            .Where(u => u.Login == primaryKey)
            .ExecuteDeleteAsync();
    }

    public override async Task<IEntity?> GetByPrimaryKey(string login)
    {
        var user = await context.Users
            .AsNoTracking()
            .Where(u => u.Login == login)
            .FirstOrDefaultAsync();
        return user;
    }

    private async Task<bool> IsInDataBaseByLogin(string login)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Login == login)
            .AnyAsync();
    }
}