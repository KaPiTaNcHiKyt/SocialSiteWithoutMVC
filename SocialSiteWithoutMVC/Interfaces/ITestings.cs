namespace SocialSiteWithoutMVC.Interfaces;

public interface ITestings
{
    public bool MainTests();
    public (bool isConfirmTest, string? resultCookie) MainTests(string cookieName);
}