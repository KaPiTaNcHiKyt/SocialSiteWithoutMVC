using Riok.Mapperly.Abstractions;

namespace SocialSiteWithoutMVC.DataAccessLayer.Models;

public record ChatEntity
{
    [MapperIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<UserEntity?> Users { get; set; } = null!;
    public List<MessageEntity>? Messages { get; set; }
}