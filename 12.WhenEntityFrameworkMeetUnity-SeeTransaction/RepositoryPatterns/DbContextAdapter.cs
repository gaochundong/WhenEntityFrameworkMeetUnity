using System.Data;
using System.Data.Entity;
using System.Data.Objects;

namespace WhenEntityFrameworkMeetUnity.RepositoryPatterns
{
  public class DbContextAdapter : IObjectSetFactory, IObjectContext
  {
    private readonly ObjectContext _context;

    public DbContextAdapter(DbContext context)
    {
      _context = context.GetObjectContext();
    }

    #region IObjectSetFactory Members

    public IObjectSet<T> CreateObjectSet<T>() where T : class
    {
      return _context.CreateObjectSet<T>();
    }

    public void ChangeObjectState(object entity, EntityState state)
    {
      _context.ObjectStateManager.ChangeObjectState(entity, state);
    }

    public void Dispose()
    {
      _context.Dispose();
    }

    #endregion

    #region IObjectContext Members

    public void SaveChanges()
    {
      _context.SaveChanges();
    }

    #endregion
  }
}