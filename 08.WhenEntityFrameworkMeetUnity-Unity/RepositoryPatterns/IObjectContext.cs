using System;

namespace WhenEntityFrameworkMeetUnity.RepositoryPatterns
{
  public interface IObjectContext : IDisposable
  {
    void SaveChanges();
  }
}