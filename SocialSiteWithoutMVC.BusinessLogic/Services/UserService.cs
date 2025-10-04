using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SocialSiteWithoutMVC.DataAccessLayer;
using SocialSiteWithoutMVC.DataAccessLayer.Models;
using SocialSiteWithoutMVC.infrastructureLogic.Services;

namespace SocialSiteWithoutMVC.BusinessLogic.Services;

public class UserService(JwtService jwtService, SocialSiteDbContext context, IMemoryCache cache)
{
    public async Task<bool> Add(string login, string password, string nickname)
    {
        if (await context.Users
            .AsNoTracking()
            .Where(u => u.Login == login)
            .AnyAsync())
        {
            return false;
        }
        var user = new UserEntity
        {
            Login = login,
            Password = password,
            Nickname = nickname
        };
        user.Password = new PasswordHasher<UserEntity>().HashPassword(user, password);
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<(bool, string?)> Login(string login, string password)
    {
        var user = await GetUser(login);
        if (user is null)
        {
            return (false, null);
        }
        var hasher = new PasswordHasher<UserEntity>()
            .VerifyHashedPassword(user, user.Password, password);
        if (hasher != PasswordVerificationResult.Success) 
            return (false, null);
        var token = jwtService.GenerateToken(user);
        return (true, token);
    }

    public async Task Delete(string login, string password)
    {
        var userInDb = await GetUser(login);
        if (userInDb is null)
            return;
        var hasher = new PasswordHasher<UserEntity>()
            .VerifyHashedPassword(userInDb, userInDb.Password, password);
        if (hasher != PasswordVerificationResult.Success)
            return;
        await context.Users
            .Where(u => u.Login == login)
            .ExecuteDeleteAsync();
    }

    public async Task<UserEntity[]?> GetByFilter(string filter)
    {
        if (cache.TryGetValue($"{filter}_filter", out UserEntity[]? users))
            return users;
        users = await context.Users
            .AsNoTracking()
            .Where(u => u.Login.Contains(filter))
            .ToArrayAsync();
        if (users.Length != 0)
        {
            cache.Set($"{filter}_filter", users, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
        }
        return users;
    }

    public async Task<UserEntity?> GetUser(string login)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Login == login)
            .FirstOrDefaultAsync();
    }
}