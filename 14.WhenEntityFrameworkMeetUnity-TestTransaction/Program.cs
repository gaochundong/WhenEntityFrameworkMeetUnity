using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using WhenEntityFrameworkMeetUnity.DataAccess;

namespace WhenEntityFrameworkMeetUnity
{
  class Program
  {
    static void Main(string[] args)
    {
      ICustomerRepository customerRepository = new CustomerRepository();

      // =============== 增 ===============
      Console.ForegroundColor = ConsoleColor.Red;

      DomainModels.Customer customer1 = new DomainModels.Customer()
      {
        Name = "Dennis Gao",
        Address = "Beijing",
        Phone = "18888888888",
      };
      DomainModels.Customer customer2 = new DomainModels.Customer()
      {
        //Name = "Degang Guo", // 创造一个无效的对象，此处客户名称不能为空
        Address = "Beijing",
        Phone = "16666666666",
      };

      //Task t = Task.Factory.StartNew(() =>
      //{
      //  int i = 0;
      //  while (i < 100)
      //  {
      //    List<DomainModels.Customer> customers = new List<DomainModels.Customer>();

      //    using (var transactionScope = new TransactionScope(
      //      TransactionScopeOption.RequiresNew,
      //      new TransactionOptions()
      //      {
      //        IsolationLevel = IsolationLevel.ReadUncommitted
      //      }))
      //    {
      //      try
      //      {
      //        using (RetailEntities context = new RetailEntities())
      //        {
      //          List<Customer> entities = context.Customers.AsQueryable().ToList();

      //          foreach (var entity in entities)
      //          {
      //            DomainModels.Customer customer = Mapper.Map<Customer, DomainModels.Customer>(entity);
      //            customers.Add(customer);
      //          }
      //        }
      //      }
      //      catch (Exception ex)
      //      {
      //        Console.WriteLine(FlattenException(ex));
      //      }
      //      transactionScope.Complete();
      //    }

      //    Console.WriteLine("-----> " + i + " times");
      //    foreach (var customer in customers)
      //    {
      //      Console.WriteLine(customer);
      //    }

      //    i++;
      //    Thread.Sleep(1000);
      //  }
      //});

      try
      {
        using (var transactionScope = new TransactionScope(
          TransactionScopeOption.RequiresNew))
        {
          Customer entity1 = Mapper.Map<DomainModels.Customer, Customer>(customer1);
          Customer entity2 = Mapper.Map<DomainModels.Customer, Customer>(customer2);

          using (RetailEntities context = new RetailEntities())
          {
            context.Customers.Add(entity1);
            context.SaveChanges(); // 顺利提交
            context.Customers.Add(entity2);
            context.SaveChanges(); // 提交时将抛出异常

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
      List<DomainModels.Customer> getCustomers = new List<DomainModels.Customer>();

      using (var transactionScope = new TransactionScope(
        TransactionScopeOption.RequiresNew,
        new TransactionOptions()
        {
          IsolationLevel = IsolationLevel.ReadUncommitted
        }))
      {
        try
        {
          using (RetailEntities context = new RetailEntities())
          {
            List<Customer> entities = context.Customers.AsQueryable().ToList();

            foreach (var entity in entities)
            {
              DomainModels.Customer customer = Mapper.Map<Customer, DomainModels.Customer>(entity);
              getCustomers.Add(customer);
            }
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine(FlattenException(ex));
        }

        transactionScope.Complete();
      }

      foreach (var customer in getCustomers)
      {
        Console.WriteLine(customer);
      }

      // =============== 清理 ===============
      Console.WriteLine();
      Console.WriteLine("Press any key to clean database...");
      Console.ReadKey();

      customerRepository.DeleteAllCustomers();
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
