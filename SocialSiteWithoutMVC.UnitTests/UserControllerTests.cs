using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace SocialSiteWithoutMVC.UnitTests;

[TestFixture]
public class UserControllerTests
{
    public async Task<IActionResult> PostUser_SendRequest_ShouldReturnIActionResult()
    {
        //Arrange
        WebApplicationFactory<Program> factory = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        var client = factory.CreateClient();
        
        //Act
        var response = await client.PostAsync("api/UserController/PostUser", );

        //Assert
    }
}