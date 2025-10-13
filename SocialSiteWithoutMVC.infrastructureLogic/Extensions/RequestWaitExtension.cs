using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;

namespace SocialSiteWithoutMVC.InfrastructureLogic.Extensions;

public static class RequestWaitExtension
{
    public static void AddLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(o =>
        {
            o.AddFixedWindowLimiter("Default", opt =>
            {
                opt.PermitLimit = 1;
                opt.Window = TimeSpan.FromSeconds(1);
            });
        });
        services.AddRateLimiter(o =>
        {
            o.AddFixedWindowLimiter("Edit", opt =>
            {
                opt.PermitLimit = 3;
                opt.Window = TimeSpan.FromMinutes(1);
            });
        });
    }
}