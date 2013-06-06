using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;

namespace WhenEntityFrameworkMeetUnity.RepositoryPatterns
{
  public static class DbContextExtensions
  {
    public static ObjectContext GetObjectContext(this DbContext dbContext)
    {
      return ((IObjectContextAdapter)dbContext).ObjectContext;
    }
  }
}