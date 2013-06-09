using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
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

      try
      {

        Customer entity1 = Mapper.Map<DomainModels.Customer, Customer>(customer1);
        Customer entity2 = Mapper.Map<DomainModels.Customer, Customer>(customer2);

        using (RetailEntities context = new RetailEntities())
        {
          context.Customers.Add(entity1);
          context.Customers.Add(entity2);

          using (var transactionScope = new TransactionScope(
            TransactionScopeOption.RequiresNew,
            new TransactionOptions()
            {
              IsolationLevel = IsolationLevel.ReadUncommitted
            }))
          {
            context.SaveChanges();
            transactionScope.Complete();
          }

          customer1.Id = entity1.Id;
          customer2.Id = entity2.Id;
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

      try
      {
        using (var transactionScope = new TransactionScope(
          TransactionScopeOption.RequiresNew,
          new TransactionOptions()
          {
            IsolationLevel = IsolationLevel.ReadUncommitted
          }))
        {
          using (RetailEntities context = new RetailEntities())
          {
            List<Customer> entities = context.Customers.AsQueryable().ToList();

            foreach (var entity in entities)
            {
              DomainModels.Customer customer = Mapper.Map<Customer, DomainModels.Customer>(entity);
              customers.Add(customer);
            }
          }
          transactionScope.Complete();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(FlattenException(ex));
      }

      foreach (var customer in customers)
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
