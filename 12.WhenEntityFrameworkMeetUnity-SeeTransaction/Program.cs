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

      Mapper.CreateMap<DomainModels.Customer, Customer>();
      Mapper.CreateMap<Customer, DomainModels.Customer>();
      Mapper.CreateMap<DomainModels.Supplier, Supplier>();
      Mapper.CreateMap<Supplier, DomainModels.Supplier>();

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
        Customer entity = Mapper.Map<DomainModels.Customer, Customer>(customer1);

        Repository.Customers.Insert(entity);
        Repository.Commit();

        customer1.Id = entity.Id;
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
        Customer entity = Mapper.Map<DomainModels.Customer, Customer>(customer2);

        Repository.Customers.Insert(entity);
        Repository.Commit();

        customer2.Id = entity.Id;
      }
      Console.WriteLine(customer2);

      DomainModels.Supplier supplier1 = new DomainModels.Supplier()
      {
        CompanyName = "Microsoft",
        Address = "Beijing",
        ContactName = "Bill Gates",
        ContactTitle = "CEO",
      };
      using (UnityContainerScope scope = UnityContainerScope.NewScope())
      {
        Supplier entity = Mapper.Map<DomainModels.Supplier, Supplier>(supplier1);

        Repository.Suppliers.Insert(entity);
        Repository.Commit();

        supplier1.Id = entity.Id;
      }
      Console.WriteLine(supplier1);

      // =============== 事务 ===============
      Console.ForegroundColor = ConsoleColor.Green;

      using (UnityContainerScope scope = UnityContainerScope.NewScope())
      {
        customer1.Address = Guid.NewGuid().ToString();
        customer2.Address = Guid.NewGuid().ToString();
        supplier1.ContactName = "Steven Ballmer";

        Customer entity1 = Mapper.Map<DomainModels.Customer, Customer>(customer1);
        Customer entity2 = Mapper.Map<DomainModels.Customer, Customer>(customer2);
        Supplier entity3 = Mapper.Map<DomainModels.Supplier, Supplier>(supplier1);

        Repository.Customers.Update(entity1); // good, we are in same transaction
        Repository.Customers.Update(entity2); // good, we are in same transaction
        Repository.Suppliers.Update(entity3); // good, we are in same transaction
        Repository.Commit();
      }

      // =============== 清理 ===============
      Console.WriteLine();
      Console.WriteLine("Press any key to clean database...");
      Console.ReadKey();

      using (UnityContainerScope scope = UnityContainerScope.NewScope())
      {
        List<Customer> entities1 = Repository.Customers.Query().ToList();
        List<Supplier> entities2 = Repository.Suppliers.Query().ToList();

        foreach (var entity in entities1)
        {
          Repository.Customers.Delete(entity);
        }
        foreach (var entity in entities2)
        {
          Repository.Suppliers.Delete(entity);
        }

        Repository.Commit();
      }
    }
  }
}
