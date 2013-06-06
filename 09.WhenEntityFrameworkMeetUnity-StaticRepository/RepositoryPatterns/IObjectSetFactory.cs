using System;
using System.Data;
using System.Data.Objects;

namespace WhenEntityFrameworkMeetUnity.RepositoryPatterns
{
  public interface IObjectSetFactory : IDisposable
  {
    IObjectSet<T> CreateObjectSet<T>() where T : class;
    void ChangeObjectState(object entity, EntityState state);
  }
}