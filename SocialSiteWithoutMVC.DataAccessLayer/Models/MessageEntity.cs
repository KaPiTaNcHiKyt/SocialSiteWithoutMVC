using Riok.Mapperly.Abstractions;

namespace SocialSiteWithoutMVC.DataAccessLayer.Models;

public record MessageEntity
{
    [MapperIgnore]
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
    public string Text { get; set; } = null!;
    [MapperIgnore]
    public string ChatName { get; set; } = null!;
    public string UserLogin { get; set; } = null!;
}