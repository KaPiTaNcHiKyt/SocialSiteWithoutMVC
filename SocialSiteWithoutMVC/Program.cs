using Microsoft.EntityFrameworkCore;
using SocialSiteWithoutMVC;
using SocialSiteWithoutMVC.DataAccessLayer;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddDbContext<SocialSiteDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString(nameof(SocialSiteDbContext)));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.UseSwagger()
    .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SocialSite v1"));

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();