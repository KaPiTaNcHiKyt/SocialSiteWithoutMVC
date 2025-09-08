using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SocialSiteWithoutMVC.DataAccessLayer.Interfaces;
using SocialSiteWithoutMVC.DataAccessLayer.Models;

namespace SocialSiteWithoutMVC.DataAccessLayer.Repositories;

public class UserRepository(SocialSiteDbContext context) : IRepository
{
    public async Task AddNew(UserEntity user)
    {
        await context.Users
                .AddAsync(user);
        
        await context.SaveChangesAsync();
    }

    public async Task UpdateUser(string loginNow,
        Expression<Func<SetPropertyCalls<UserEntity>, SetPropertyCalls<UserEntity>>> setPropertyCalls)
    {
        await context.Users
            .Where(u => u.Login == loginNow)
            .ExecuteUpdateAsync(setPropertyCalls);
    }

    public async Task DeleteByDate(UserEntity user)
    {
        await context.Users
            .Where(u => u.Login == user.Login && u.Password == user.Password)
            .ExecuteDeleteAsync();
    }

    public async Task<UserEntity?> GetByLogin(string login)
    {
        var user = await context.Users
            .AsNoTracking()
            .Include(u => u.Chats)
            .Where(u => u.Login == login)
            .FirstOrDefaultAsync();
        return user;
    }
    
    public async Task<UserEntity[]?> GetByFilter(string filter)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Login.Contains(filter))
            .ToArrayAsync();
    }

    public async Task<bool> IsInDataBase(string login)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Login == login)
            .AnyAsync();
    }
}