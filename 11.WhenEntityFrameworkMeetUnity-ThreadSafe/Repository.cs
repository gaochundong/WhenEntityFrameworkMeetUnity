using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhenEntityFrameworkMeetUnity.RepositoryPatterns;
using WhenEntityFrameworkMeetUnity.Models;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace WhenEntityFrameworkMeetUnity
{
  public static class Repository
  {
    public static IRepository<Customer> Customers
    {
      get { return UnityContainerDispatcher.GetContainer().Resolve<IRepository<Customer>>(); }
    }

    public static void Commit()
    {
      UnityContainerDispatcher.GetContainer().Resolve<IUnitOfWork>().Commit();
    }
  }
}
