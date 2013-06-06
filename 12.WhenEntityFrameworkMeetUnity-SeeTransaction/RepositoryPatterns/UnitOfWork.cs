using System;

namespace WhenEntityFrameworkMeetUnity.RepositoryPatterns
{
  public class UnitOfWork : IUnitOfWork, IDisposable
  {
    private readonly IObjectContext _objectContext;

    public UnitOfWork(IObjectContext objectContext)
    {
      _objectContext = objectContext;
    }

    #region IUnitOfWork Members

    public void Commit()
    {
      _objectContext.SaveChanges();
    }

    #endregion

    #region IDisposable Members

    public void Dispose()
    {
      if (_objectContext != null)
      {
        _objectContext.Dispose();
      }

      GC.SuppressFinalize(this);
    }

    #endregion
  }
}