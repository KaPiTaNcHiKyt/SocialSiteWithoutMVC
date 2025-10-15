namespace SocialSiteWithoutMVC.InfrastructureLogic.Settings;

public class AuthSettings
{
    public TimeSpan Expires { get; set; }
    public string SecretKey { get; set; }
}