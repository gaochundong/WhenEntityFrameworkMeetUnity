using System.Linq;

namespace WhenEntityFrameworkMeetUnity.RepositoryPatterns
{
  public interface IRepository<T>
    where T : class
  {
    IQueryable<T> Query();
    void Insert(T entity);
    void Update(T entity);
    void Delete(T entity);
  }
}