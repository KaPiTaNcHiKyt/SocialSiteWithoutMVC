using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialSiteWithoutMVC.BusinessLogic.Settings;
using SocialSiteWithoutMVC.DataAccessLayer.Models;

namespace SocialSiteWithoutMVC.BusinessLogic.Services;

public class JwtService(IOptions<AuthSettings> options)
{
    public string GenerateToken(UserEntity user)
    {
        var claim = new List<Claim>
        {
            new("Nickname", user.NickName),
            new("Login", user.Login)
        };
        
        var jwtToken = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(options.Value.Expires),
            claims: claim,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey)), 
                SecurityAlgorithms.HmacSha256));
        
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    public string? GetLogin(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        
        var jwtToken = handler.ReadJwtToken(token);
        
        return jwtToken.Claims
            .Where(c => c.Type == "Login")
            .Select(c => c.Value)
            .FirstOrDefault();
    }
}