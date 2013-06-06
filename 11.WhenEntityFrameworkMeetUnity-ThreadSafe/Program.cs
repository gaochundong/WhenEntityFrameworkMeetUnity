using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using WhenEntityFrameworkMeetUnity.DataAccess;
using WhenEntityFrameworkMeetUnity.Models;
using WhenEntityFrameworkMeetUnity.RepositoryPatterns;

namespace WhenEntityFrameworkMeetUnity
{
  class Program
  {
    static void Main(string[] args)
    {
      IUnityContainer container = new UnityContainer()
        .RegisterType(typeof(IRepository<>), typeof(Repository<>), new HierarchicalLifetimeManager())
        .RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager())
        .RegisterType<DbContext, RETAILContext>(new HierarchicalLifetimeManager())
        .RegisterType<DbContextAdapter>(new HierarchicalLifetimeManager())
        .RegisterType<IObjectSetFactory, DbContextAdapter>(new HierarchicalLifetimeManager())
        .RegisterType<IObjectContext, DbContextAdapter>(new HierarchicalLifetimeManager())
        .RegisterType<ICustomerRepository, CustomerRepository>(new HierarchicalLifetimeManager());

      UnityContainerDispatcher.InjectParentContainer(container);

      ICustomerRepository customerRepository = container.Resolve<ICustomerRepository>();

      // =============== 增 ===============
      Console.ForegroundColor = ConsoleColor.DarkRed;

      DomainModels.Customer customer1 = new DomainModels.Customer()
      {
        Name = "Dennis Gao",
        Address = "Beijing",
        Phone = "18888888888",
      };
      using (UnityContainerScope scope = UnityContainerScope.NewScope())
      {
        customerRepository.InsertCustomer(customer1);
      }
      Console.WriteLine(customer1);

      DomainModels.Customer customer2 = new DomainModels.Customer()
      {
        Name = "Degang Guo",
        Address = "Beijing",
        Phone = "16666666666",
      };
      using (UnityContainerScope scope = UnityContainerScope.NewScope())
      {
        customerRepository.InsertCustomer(customer2);
      }
      Console.WriteLine(customer2);

      // =============== 并发 ===============
      Console.ForegroundColor = ConsoleColor.Green;
      Mapper.CreateMap<DomainModels.Customer, DomainModels.Customer>();
      List<Task> tasks = new List<Task>();
      for (int i = 0; i < 16; i++)
      {
        DomainModels.Customer modifiedCustomer = Mapper.Map<DomainModels.Customer, DomainModels.Customer>(customer1);
        modifiedCustomer.Name = modifiedCustomer.Name + i;

        Task t = Task.Factory.StartNew(() =>
        {
          try
          {
            using (UnityContainerScope scope = UnityContainerScope.NewScope())
            {
              customerRepository.UpdateCustomer(modifiedCustomer);
              Console.WriteLine("Modified " + modifiedCustomer.Name + " in thread " + Thread.CurrentThread.ManagedThreadId);
            }
          }
          catch (Exception ex)
          {
            Console.WriteLine("Exception occurred in thread " + Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine(ex.Message);
          }
        });
        tasks.Add(t);
      }
      Task.WaitAll(tasks.ToArray());

      // =============== 清理 ===============
      Console.WriteLine();
      Console.WriteLine("Press any key to clean database...");
      Console.ReadKey();

      using (UnityContainerScope scope = UnityContainerScope.NewScope())
      {
        customerRepository.DeleteAllCustomers();
      }
    }
  }
}
