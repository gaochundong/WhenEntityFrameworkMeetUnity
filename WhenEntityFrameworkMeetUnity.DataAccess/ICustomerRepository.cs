using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhenEntityFrameworkMeetUnity.DomainModels;

namespace WhenEntityFrameworkMeetUnity.DataAccess
{
  public interface ICustomerRepository
  {
    void InsertCustomer(Customer customer);
    void UpdateCustomer(Customer customer);
    List<Customer> GetAllCustomers();
    List<Customer> GetCustomersByAddress(string address);
    void DeleteAllCustomers();
    void DeleteCustomersByAddress(string address);
  }
}
