using Riok.Mapperly.Abstractions;

namespace SocialSiteWithoutMVC.DataAccessLayer.Models;

public record MessageEntity
{
    [MapperIgnore]
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
    public string Text { get; set; } = null!;
    [MapperIgnore]
    public Guid ChatId { get; set; }
    public string UserLogin { get; set; } = null!;
}