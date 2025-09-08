using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using SocialSiteWithoutMVC.BusinessLogic;
using SocialSiteWithoutMVC.BusinessLogic.Services;
using SocialSiteWithoutMVC.BusinessLogic.Settings;
using SocialSiteWithoutMVC.DataAccessLayer;
using SocialSiteWithoutMVC.DataAccessLayer.Repositories;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));

builder.Services.AddControllers();
builder.Services.AddDbContext<SocialSiteDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString(nameof(SocialSiteDbContext)));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AuthSettings>(
    builder.Configuration.GetSection("AuthSettings"));

builder.Services.AddAuthorization();

builder.Services.AddApiAuthentification(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ChatRepository>();
builder.Services.AddScoped<MessageRepository>();

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<EditUserService>();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); 

app.UseHttpsRedirection();
app.UseRouting();

app.UseSwagger()
    .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SocialSite v1"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();