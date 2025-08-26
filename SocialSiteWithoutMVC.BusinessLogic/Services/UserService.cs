using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialSiteWithoutMVC.DataAccessLayer.Models;
using Dal = SocialSiteWithoutMVC.DataAccessLayer;

namespace SocialSiteWithoutMVC.BusinessLogic.Services;

public class UserService(Dal.SocialSiteDbContext context)
{
    public async Task<bool> AddUser(string login, string password, string nickname)
    {
        var user = new UserEntity
        {
            Login = login,
            Password = password,
            NickName = nickname
        };
        user.Password = new PasswordHasher<UserEntity>().HashPassword(user, password);
        var repository = new Dal.Repositories.UserRepository(context);
        
        return await repository.AddNew(user);
    }

    public async Task<UserEntity?> GetUserByPassword(string login, string password)
    {
        var repository = new DataAccessLayer.Repositories.UserRepository(context);

        if (await repository.GetByPrimaryKey(login) is not UserEntity user) 
            return null;
        
        var hasher = new PasswordHasher<UserEntity>().VerifyHashedPassword(user, user.Password, password);
        
        return hasher == PasswordVerificationResult.Success ? user : null;
    }
}