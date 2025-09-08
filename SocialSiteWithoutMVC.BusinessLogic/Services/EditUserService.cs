using Microsoft.AspNetCore.Identity;
using SocialSiteWithoutMVC.DataAccessLayer.Models;
using SocialSiteWithoutMVC.DataAccessLayer.Repositories;

namespace SocialSiteWithoutMVC.BusinessLogic.Services;

public class EditUserService(UserRepository repository)
{
    public async Task Patch(string loginNow, string login, string password, string nickName)
    {
        var user = new UserEntity
        {
            Login = login,
            Password = password,
            NickName = nickName
        };
        
        if (password != string.Empty)
        {
            var hashPassword = new PasswordHasher<UserEntity>().HashPassword(user, password);
            user.Password = hashPassword;
            
            await repository.Patch(loginNow, login, password, nickName);
        }
        
    }

    public async Task<bool> PatchLogin(string loginNow, string newLogin)
    {
        if (loginNow == newLogin || await repository.IsInDataBase(newLogin))
            return false;
        
        await repository.UpdateUser(loginNow, s => s
            .SetProperty(u => u.Login, newLogin));
        
        return true;
    }

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