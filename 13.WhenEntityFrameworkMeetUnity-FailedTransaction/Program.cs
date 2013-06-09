using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
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
        .RegisterType<IObjectContext, DbContextAdapter>(new HierarchicalLifetimeManager());

      UnityContainerDispatcher.InjectParentContainer(container);

      Mapper.CreateMap<DomainModels.Customer, Customer>();
      Mapper.CreateMap<Customer, DomainModels.Customer>();

      // =============== 测试事务回滚 ===============
      Console.ForegroundColor = ConsoleColor.Red;

      DomainModels.Customer customer1 = new DomainModels.Customer()
      {
        Name = "Dennis Gao",
        Address = "Beijing",
        Phone = "18888888888",
      };
      DomainModels.Customer customer2 = new DomainModels.Customer()
      {
        //Name = null, // 创造一个无效的对象，此处客户名称不能为空
        Address = "Beijing",
        Phone = "16666666666",
      };

      try
      {
        using (var transactionScope = new TransactionScope(
          TransactionScopeOption.RequiresNew,
          new TransactionOptions()
          {
            IsolationLevel = IsolationLevel.ReadUncommitted
          }))
        {
          using (UnityContainerScope scope = UnityContainerScope.NewScope())
          {
            Customer entity1 = Mapper.Map<DomainModels.Customer, Customer>(customer1);
            Customer entity2 = Mapper.Map<DomainModels.Customer, Customer>(customer2);

            Repository.Customers.Insert(entity1);
            Repository.Customers.Insert(entity2);
            Repository.Commit();

            customer1.Id = entity1.Id;
            customer2.Id = entity2.Id;
          }

          transactionScope.Complete();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(FlattenException(ex));
      }

      Console.WriteLine(customer1);
      Console.WriteLine(customer2);
      Console.WriteLine("=====================================");

      // =============== 查询回滚结果 ===============
      List<DomainModels.Customer> customers = new List<DomainModels.Customer>();

      using (var transactionScope = new TransactionScope(
        TransactionScopeOption.RequiresNew,
        new TransactionOptions()
        {
          IsolationLevel = IsolationLevel.ReadCommitted
        }))
      {
        using (UnityContainerScope scope = UnityContainerScope.NewScope())
        {
          List<Customer> entities = Repository.Customers.Query().ToList();

          foreach (var entity in entities)
          {
            DomainModels.Customer customer = Mapper.Map<Customer, DomainModels.Customer>(entity);
            customers.Add(customer);
          }
        }
        transactionScope.Complete();
      }

      foreach (var customer in customers)
      {
        Console.WriteLine(customer);
      }

      // =============== 清理 ===============
      Console.WriteLine();
      Console.WriteLine("Press any key to clean database...");
      Console.ReadKey();

      using (UnityContainerScope scope = UnityContainerScope.NewScope())
      {
        List<Customer> entities1 = Repository.Customers.Query().ToList();

        foreach (var entity in entities1)
        {
          Repository.Customers.Delete(entity);
        }

        Repository.Commit();
      }
    }

    public static string FlattenException(Exception exception)
    {
      var stringBuilder = new StringBuilder();

      while (exception != null)
      {
        stringBuilder.AppendLine(exception.Message);
        stringBuilder.AppendLine(exception.StackTrace);

        exception = exception.InnerException;
      }

      return stringBuilder.ToString();
    }
  }
}
