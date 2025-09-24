using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SocialSiteWithoutMVC.DataAccessLayer;
using SocialSiteWithoutMVC.DataAccessLayer.Models;

namespace SocialSiteWithoutMVC.BusinessLogic.Services;

public class EditUserService(SocialSiteDbContext context)
{
    public async Task PatchNickname(string loginNow, string newNickName)
    {
        await UpdateUser(loginNow, s => s
            .SetProperty(u => u.Nickname, newNickName));
    }
    
    public async Task PatchPassword(string loginNow, string newPassword)
    {
        var user = new UserEntity
        {
            Password = newPassword
        };
        var hashPassword = new PasswordHasher<UserEntity>().HashPassword(user, newPassword);
        await UpdateUser(loginNow, s => s
            .SetProperty(u => u.Password, hashPassword));
    }
    private async Task UpdateUser(string loginNow,
        Expression<Func<SetPropertyCalls<UserEntity>, SetPropertyCalls<UserEntity>>> setPropertyCalls)
    {
        await context.Users
            .Where(u => u.Login == loginNow)
            .ExecuteUpdateAsync(setPropertyCalls);
    }
}