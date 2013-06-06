using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Text;
using WhenEntityFrameworkMeetUnity.RepositoryPatterns;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using WhenEntityFrameworkMeetUnity.DataAccess;
using WhenEntityFrameworkMeetUnity.Models;

namespace WhenEntityFrameworkMeetUnity
{
  class Program
  {
    static void Main(string[] args)
    {
      IUnityContainer container = new UnityContainer()
        .RegisterType(typeof(IRepository<>), typeof(Repository<>), new ContainerControlledLifetimeManager())
        .RegisterType<IUnitOfWork, UnitOfWork>(new ContainerControlledLifetimeManager())
        .RegisterType<DbContext, RETAILContext>(new ContainerControlledLifetimeManager())
        .RegisterType<DbContextAdapter>(new ContainerControlledLifetimeManager())
        .RegisterType<IObjectSetFactory, DbContextAdapter>(new ContainerControlledLifetimeManager())
        .RegisterType<IObjectContext, DbContextAdapter>(new ContainerControlledLifetimeManager())
        .RegisterType<ICustomerRepository, CustomerRepository>(new ContainerControlledLifetimeManager());

      UnityServiceLocator locator = new UnityServiceLocator(container);
      ServiceLocator.SetLocatorProvider(() => locator);

      ICustomerRepository customerRepository = container.Resolve<ICustomerRepository>();

      // =============== 增 ===============
      Console.ForegroundColor = ConsoleColor.DarkRed;

      DomainModels.Customer customer1 = new DomainModels.Customer()
      {
        Name = "Dennis Gao",
        Address = "Beijing",
        Phone = "18888888888",
      };
      customerRepository.InsertCustomer(customer1);
      Console.WriteLine(customer1);

      DomainModels.Customer customer2 = new DomainModels.Customer()
      {
        Name = "Degang Guo",
        Address = "Beijing",
        Phone = "16666666666",
      };
      customerRepository.InsertCustomer(customer2);
      Console.WriteLine(customer2);

      // =============== 查 ===============
      Console.WriteLine();
      Console.ForegroundColor = ConsoleColor.DarkYellow;

      List<DomainModels.Customer> allCustomers = customerRepository.GetAllCustomers();
      foreach (var customer in allCustomers)
      {
        Console.WriteLine(customer);
      }

      // =============== 改 ===============
      Console.WriteLine();
      Console.ForegroundColor = ConsoleColor.Green;

      customer2.Address = "Tianjin";
      customerRepository.UpdateCustomer(customer2);
      Console.WriteLine(customer2);

      // =============== 查 ===============
      Console.WriteLine();
      Console.ForegroundColor = ConsoleColor.Cyan;

      List<DomainModels.Customer> filteredCustomers = customerRepository.GetCustomersByAddress("Tianjin");
      foreach (var customer in filteredCustomers)
      {
        Console.WriteLine(customer);
      }

      // =============== 删 ===============
      Console.WriteLine();
      Console.ForegroundColor = ConsoleColor.Blue;

      customerRepository.DeleteCustomersByAddress("Tianjin");

      // =============== 查 ===============
      Console.WriteLine();
      Console.ForegroundColor = ConsoleColor.Magenta;

      List<DomainModels.Customer> existingCustomers = customerRepository.GetAllCustomers();
      foreach (var customer in existingCustomers)
      {
        Console.WriteLine(customer);
      }

      // =============== 清理 ===============
      Console.WriteLine();
      Console.WriteLine("Press any key to clean database...");
      Console.ReadKey();

      customerRepository.DeleteAllCustomers();
    }
  }
}
