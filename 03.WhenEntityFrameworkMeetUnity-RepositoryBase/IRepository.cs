using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhenEntityFrameworkMeetUnity
{
  public interface IRepository<T> where T : class
  {
    T Add(T entity);
    T Update(T entity);
    IList<T> GetAll();
    void RemoveAll();
  }
}
