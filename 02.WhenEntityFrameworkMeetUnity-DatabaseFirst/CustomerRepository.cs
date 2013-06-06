using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using WhenEntityFrameworkMeetUnity.DataAccess;

namespace WhenEntityFrameworkMeetUnity
{
  public class CustomerRepository : ICustomerRepository
  {
    public CustomerRepository()
    {
      Mapper.CreateMap<DomainModels.Customer, Customer>();
      Mapper.CreateMap<Customer, DomainModels.Customer>();
    }

    #region ICustomerRepository Members

    public void InsertCustomer(DomainModels.Customer customer)
    {
      using (RetailEntities context = new RetailEntities())
      {
        Customer entity = Mapper.Map<DomainModels.Customer, Customer>(customer);
        context.Customers.AddObject(entity);
        context.SaveChanges();

        customer.Id = entity.Id;
      }
    }

    public void UpdateCustomer(DomainModels.Customer customer)
    {
      using (RetailEntities context = new RetailEntities())
      {
        Customer entity = context.Customers.AsQueryable().Single(c => c.Id == customer.Id);

        entity.Name = customer.Name;
        entity.Address = customer.Address;
        entity.Phone = customer.Phone;

        context.SaveChanges();
      }
    }

    public List<DomainModels.Customer> GetAllCustomers()
    {
      using (RetailEntities context = new RetailEntities())
      {
        List<Customer> entities = context.Customers.AsQueryable().ToList();
        List<DomainModels.Customer> customers = new List<DomainModels.Customer>();

        foreach (var entity in entities)
        {
          DomainModels.Customer customer = Mapper.Map<Customer, DomainModels.Customer>(entity);
          customers.Add(customer);
        }

        return customers;
      }
    }

    public List<DomainModels.Customer> GetCustomersByAddress(string address)
    {
      using (RetailEntities context = new RetailEntities())
      {
        List<Customer> entities = context.Customers.AsQueryable().Where(c => c.Address == address).ToList();
        List<DomainModels.Customer> customers = new List<DomainModels.Customer>();

        foreach (var entity in entities)
        {
          DomainModels.Customer customer = Mapper.Map<Customer, DomainModels.Customer>(entity);
          customers.Add(customer);
        }

        return customers;
      }
    }

    public void DeleteAllCustomers()
    {
      using (RetailEntities context = new RetailEntities())
      {
        List<Customer> entities = context.Customers.AsQueryable().ToList();

        foreach (var entity in entities)
        {
          context.DeleteObject(entity);
        }

        context.SaveChanges();

        //context.ExecuteStoreCommand("TRUNCATE TABLE [RETAIL].[STORE].[Customer]");
      }
    }

    public void DeleteCustomersByAddress(string address)
    {
      using (RetailEntities context = new RetailEntities())
      {
        List<Customer> entities = context.Customers.AsQueryable().Where(c => c.Address == address).ToList();

        foreach (var entity in entities)
        {
          context.DeleteObject(entity);
        }

        context.SaveChanges();
      }
    }

    #endregion
  }
}
