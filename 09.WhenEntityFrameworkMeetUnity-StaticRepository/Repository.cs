using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhenEntityFrameworkMeetUnity.RepositoryPatterns;
using WhenEntityFrameworkMeetUnity.Models;
using Microsoft.Practices.ServiceLocation;

namespace WhenEntityFrameworkMeetUnity
{
  public static class Repository
  {
    public static IRepository<Customer> Customers
    {
      get { return ServiceLocator.Current.GetInstance<IRepository<Customer>>(); }
    }

    public static void Commit()
    {
      ServiceLocator.Current.GetInstance<IUnitOfWork>().Commit();
    }

    public static IRepository<Supplier> Suppliers
    {
      get { return ServiceLocator.Current.GetInstance<IRepository<Supplier>>(); }
    }
  }
}
