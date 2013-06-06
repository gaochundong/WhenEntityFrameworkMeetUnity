using System.Data;
using System.Data.Objects;
using System.Linq;

namespace WhenEntityFrameworkMeetUnity.RepositoryPatterns
{
  public class Repository<T> : IRepository<T> where T : class
  {
    private readonly IObjectSetFactory _objectSetFactory;
    private readonly IObjectSet<T> _objectSet;

    public Repository(IObjectSetFactory objectSetFactory)
    {
      _objectSetFactory = objectSetFactory;
      _objectSet = objectSetFactory.CreateObjectSet<T>();
    }

    #region IRepository<T> Members

    public IQueryable<T> Query()
    {
      return _objectSet;
    }

    public void Insert(T entity)
    {
      _objectSet.AddObject(entity);
    }

    public void Update(T entity)
    {
      _objectSet.Attach(entity);
      _objectSetFactory.ChangeObjectState(entity, EntityState.Modified);
    }

    public void Delete(T entity)
    {
      _objectSet.DeleteObject(entity);
    }

    #endregion
  }
}