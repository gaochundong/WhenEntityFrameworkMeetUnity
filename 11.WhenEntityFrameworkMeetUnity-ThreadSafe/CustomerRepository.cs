using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using AutoMapper;
using WhenEntityFrameworkMeetUnity.RepositoryPatterns;
using WhenEntityFrameworkMeetUnity.DataAccess;
using WhenEntityFrameworkMeetUnity.Models;
using Microsoft.Practices.Unity;

namespace WhenEntityFrameworkMeetUnity
{
  public class CustomerRepository : ICustomerRepository
  {
    public CustomerRepository(IUnityContainer container)
    {
      Mapper.CreateMap<DomainModels.Customer, Customer>();
      Mapper.CreateMap<Customer, DomainModels.Customer>();
    }

    #region ICustomerRepository Members

    public void InsertCustomer(DomainModels.Customer customer)
    {
      Customer entity = Mapper.Map<DomainModels.Customer, Customer>(customer);

      Repository.Customers.Insert(entity);
      Repository.Commit();

      customer.Id = entity.Id;
    }

    public void UpdateCustomer(DomainModels.Customer customer)
    {
      Customer entity = Repository.Customers.Query().Single(c => c.Id == customer.Id);

      entity.Name = customer.Name;
      entity.Address = customer.Address;
      entity.Phone = customer.Phone;

      Repository.Customers.Update(entity);

      Repository.Commit();
    }

    public List<DomainModels.Customer> GetAllCustomers()
    {
      List<Customer> entities = Repository.Customers.Query().ToList();
      List<DomainModels.Customer> customers = new List<DomainModels.Customer>();

      foreach (var entity in entities)
      {
        DomainModels.Customer customer = Mapper.Map<Customer, DomainModels.Customer>(entity);
        customers.Add(customer);
      }

      return customers;
    }

    public List<DomainModels.Customer> GetCustomersByAddress(string address)
    {
      List<Customer> entities = Repository.Customers.Query().Where(c => c.Address == address).ToList();
      List<DomainModels.Customer> customers = new List<DomainModels.Customer>();

      foreach (var entity in entities)
      {
        DomainModels.Customer customer = Mapper.Map<Customer, DomainModels.Customer>(entity);
        customers.Add(customer);
      }

      return customers;
    }

    public void DeleteAllCustomers()
    {
      List<Customer> entities = Repository.Customers.Query().ToList();

      foreach (var entity in entities)
      {
        Repository.Customers.Delete(entity);
      }

      Repository.Commit();
    }

    public void DeleteCustomersByAddress(string address)
    {
      List<Customer> entities = Repository.Customers.Query().Where(c => c.Address == address).ToList();

      foreach (var entity in entities)
      {
        Repository.Customers.Delete(entity);
      }

      Repository.Commit();
    }

    #endregion
  }
}
