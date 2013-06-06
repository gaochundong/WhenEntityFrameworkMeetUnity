using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using AutoMapper;
using WhenEntityFrameworkMeetUnity.RepositoryPatterns;
using WhenEntityFrameworkMeetUnity.DataAccess;
using WhenEntityFrameworkMeetUnity.Models;

namespace WhenEntityFrameworkMeetUnity
{
  public class CustomerRepository : ICustomerRepository
  {
    private IRepository<Customer> _repository;
    private IUnitOfWork _uow;

    public CustomerRepository()
    {
      Mapper.CreateMap<DomainModels.Customer, Customer>();
      Mapper.CreateMap<Customer, DomainModels.Customer>();

      DbContext context = new RETAILContext();
      DbContextAdapter contextAdaptor = new DbContextAdapter(context);

      IObjectSetFactory objectSetFactory = contextAdaptor;
      _repository = new Repository<Customer>(objectSetFactory);

      IObjectContext objectContext = contextAdaptor;
      _uow = new UnitOfWork(objectContext);
    }

    #region ICustomerRepository Members

    public void InsertCustomer(DomainModels.Customer customer)
    {
      Customer entity = Mapper.Map<DomainModels.Customer, Customer>(customer);

      _repository.Insert(entity);
      _uow.Commit();

      customer.Id = entity.Id;
    }

    public void UpdateCustomer(DomainModels.Customer customer)
    {
      Customer entity = _repository.Query().Single(c => c.Id == customer.Id);

      entity.Name = customer.Name;
      entity.Address = customer.Address;
      entity.Phone = customer.Phone;

      _repository.Update(entity);

      _uow.Commit();
    }

    public List<DomainModels.Customer> GetAllCustomers()
    {
      List<Customer> entities = _repository.Query().ToList();
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
      List<Customer> entities = _repository.Query().Where(c => c.Address == address).ToList();
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
      List<Customer> entities = _repository.Query().ToList();

      foreach (var entity in entities)
      {
        _repository.Delete(entity);
      }

      _uow.Commit();
    }

    public void DeleteCustomersByAddress(string address)
    {
      List<Customer> entities = _repository.Query().Where(c => c.Address == address).ToList();

      foreach (var entity in entities)
      {
        _repository.Delete(entity);
      }

      _uow.Commit();
    }

    #endregion
  }
}
