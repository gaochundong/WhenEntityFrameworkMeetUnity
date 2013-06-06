using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;

namespace WhenEntityFrameworkMeetUnity
{
  public abstract class RepositoryBase<T> : IRepository<T> where T : EntityObject
  {
    public T Add(T entity)
    {
      T result = default(T);

      using (ObjectContext context = new RetailEntities())
      {
        context.AddObject(context.GetEntitySet<T>().Name, entity);
        context.SaveChanges();

        result = entity;
      }

      return result;
    }

    public T Update(T entity)
    {
      T result = default(T);

      using (ObjectContext context = new RetailEntities())
      {
        EntityKey key = context.CreateEntityKey(entity.EntityKey.EntitySetName, entity);

        object originEntity;
        if (context.TryGetObjectByKey(key, out originEntity))
        {
          context.ApplyCurrentValues(key.EntitySetName, entity);
          context.SaveChanges();
        }

        result = entity;
      }

      return result;
    }

    public IList<T> GetAll()
    {
      IList<T> list = new List<T>();

      using (ObjectContext context = new RetailEntities())
      {
        ObjectQuery<T> entities = context.CreateQuery<T>("[" + context.GetEntitySet<T>().Name + "]");

        foreach (var entity in entities)
        {
          list.Add(entity);
        }
      }

      return list;
    }

    public void RemoveAll()
    {
      using (ObjectContext context = new RetailEntities())
      {
        ObjectQuery<T> entities = context.CreateQuery<T>("[" + context.GetEntitySet<T>().Name + "]");

        foreach (var entity in entities)
        {
          context.DeleteObject(entity);
        }

        context.SaveChanges();
      }
    }
  }
}
