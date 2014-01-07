using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WhenEntityFrameworkMeetUnity
{
  class Program
  {
    static void Main(string[] args)
    {
      // =============== 构造数据 ===============
      Console.ForegroundColor = ConsoleColor.Green;

      int customerCount = 10000;

      List<Customer> customers = new List<Customer>();
      for (int i = 0; i < customerCount; i++)
      {
        Customer customer = new Customer()
        {
          Name = "Dennis Gao" + i,
          Address = "Beijing" + i,
          Phone = "18888888888" + i,
        };
        customers.Add(customer);

        Console.Write(".");
      }

      Console.WriteLine();

      try
      {
        // =============== 插入数据 ===============
        Console.WriteLine(string.Format(
          "Begin to insert {0} customers into database...",
          customerCount));

        Stopwatch watch = Stopwatch.StartNew();

        using (RetailEntities context = new RetailEntities())
        {
          foreach (var entity in customers)
          {
            context.Customers.Add(entity);
          }
          context.SaveChanges();
        }

        watch.Stop();
        Console.WriteLine(string.Format(
          "Done, {0} customers are inserted, cost {1} milliseconds.",
          customerCount, watch.ElapsedMilliseconds));
      }
      catch (Exception ex)
      {
        Console.WriteLine(FlattenException(ex));
      }

      Console.WriteLine("=====================================");

      // =============== 查询结果 ===============
      try
      {
        using (RetailEntities context = new RetailEntities())
        {
          int countOfCustomers = context.Customers.AsQueryable().Count();
          Console.WriteLine(string.Format(
            "Now, we have {0} customers.", countOfCustomers));
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(FlattenException(ex));
      }

      Console.WriteLine("=====================================");

      // =============== 清理 ===============
      Console.WriteLine();
      Console.WriteLine("Press any key to close...");
      Console.ReadKey();
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
