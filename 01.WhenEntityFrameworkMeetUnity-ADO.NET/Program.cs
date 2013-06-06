using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WhenEntityFrameworkMeetUnity.DataAccess;
using WhenEntityFrameworkMeetUnity.DomainModels;

namespace WhenEntityFrameworkMeetUnity
{
  class Program
  {
    static void Main(string[] args)
    {
      ICustomerRepository customerRepository = new CustomerRepository();

      // =============== 增 ===============
      Console.ForegroundColor = ConsoleColor.DarkRed;

      Customer customer1 = new Customer()
      {
        Name = "Dennis Gao",
        Address = "Beijing",
        Phone = "18888888888",
      };
      customerRepository.InsertCustomer(customer1);
      Console.WriteLine(customer1);

      Customer customer2 = new Customer()
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

      List<Customer> allCustomers = customerRepository.GetAllCustomers();
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

      List<Customer> filteredCustomers = customerRepository.GetCustomersByAddress("Tianjin");
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

      List<Customer> existingCustomers = customerRepository.GetAllCustomers();
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
