using Microsoft.EntityFrameworkCore;
using SocialSiteWithoutMVC.DataAccessLayer.Interfaces;

namespace SocialSiteWithoutMVC.DataAccessLayer.Repositories;

public abstract class SocialSiteRepository(DbContext context)
{
    public abstract Task AddNew(IEntity entity);
    public abstract Task<IEntity?> GetByPrimaryKey(string primaryKey);
    public abstract Task UpdateByPrimaryKey(IEntity entity);
    public abstract Task DeleteByPrimaryKey(string primaryKey);
}