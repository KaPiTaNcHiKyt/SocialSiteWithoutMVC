using Microsoft.AspNetCore.Identity;
using SocialSiteWithoutMVC.DataAccessLayer.Models;
using SocialSiteWithoutMVC.DataAccessLayer.Repositories;

namespace SocialSiteWithoutMVC.BusinessLogic.Services;

public class UserService(UserRepository repository, JwtService jwtService)
{
    public async Task<bool> Add(string login, string password, string nickname)
    {
        var user = new UserEntity
        {
            Login = login,
            Password = password,
            NickName = nickname
        };
        
        if (await repository.IsInDataBase(user.Login)) 
            return false;
        user.Password = new PasswordHasher<UserEntity>().HashPassword(user, password);
        
        await repository.AddNew(user);
        return true;
    }

    public async Task<(bool, string?)> Login(string login, string password)
    {
        var userInDb = await repository.GetByLogin(login);
        if (userInDb is null) 
            return (false, null);
        
        var hasher = new PasswordHasher<UserEntity>().VerifyHashedPassword(userInDb, userInDb.Password, password);

        if (hasher == PasswordVerificationResult.Success)
        {
            var token = jwtService.GenerateToken(userInDb);
            return (true, token);
        }
        return (false, null);
    }

    public async Task Delete(string login,  string password)
    {
        var userInDb = await repository.GetByLogin(login);
        if (userInDb is null)
            return;
        
        var hasher = new PasswordHasher<UserEntity>().VerifyHashedPassword(userInDb, userInDb.Password, password);

        if (hasher == PasswordVerificationResult.Success)
        {
            await repository.DeleteByDate(userInDb);
        }
    }

    public async Task<UserEntity[]?> GetByFilter(string filter)
        => await repository.GetByFilter(filter);

    public async Task<UserEntity> GetMe(string login)
        => (await repository.GetByLogin(login))!;
}