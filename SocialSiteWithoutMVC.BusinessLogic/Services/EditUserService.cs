using Microsoft.AspNetCore.Identity;
using SocialSiteWithoutMVC.DataAccessLayer.Models;
using SocialSiteWithoutMVC.DataAccessLayer.Repositories;

namespace SocialSiteWithoutMVC.BusinessLogic.Services;

public class EditUserService(UserRepository repository)
{
    public async Task PatchNickname(string loginNow, string newNickName)
        => await repository.UpdateUser(loginNow, s => s
            .SetProperty(u => u.NickName, newNickName));
    
    public async Task PatchPassword(string loginNow, string newPassword)
    {
        var user = new UserEntity
        {
            Password = newPassword
        };
        
        var hashPassword = new PasswordHasher<UserEntity>().HashPassword(user, newPassword);

        await repository.UpdateUser(loginNow, s => s
            .SetProperty(u => u.Password, hashPassword));
    }
}