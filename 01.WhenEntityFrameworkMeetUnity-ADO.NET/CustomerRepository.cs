using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WhenEntityFrameworkMeetUnity.DomainModels;
using WhenEntityFrameworkMeetUnity.DataAccess;

namespace WhenEntityFrameworkMeetUnity
{
  public class CustomerRepository : ICustomerRepository
  {
    string ConnectionString = ConfigurationManager.ConnectionStrings["RETAIL"].ConnectionString;

    public void InsertCustomer(Customer customer)
    {
      long customerId = -1;

      using (SqlConnection conn = new SqlConnection(ConnectionString))
      {
        conn.Open();
        using (SqlCommand cmd = new SqlCommand(CustomerSQL.InsertNewCustomer, conn))
        {
          cmd.Parameters.AddWithValue("@name", customer.Name);
          cmd.Parameters.AddWithValue("@address", customer.Address);
          cmd.Parameters.AddWithValue("@phone", customer.Phone);
          cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
          customerId = (long)cmd.ExecuteScalar();
        }
      }

      customer.Id = customerId;
    }

    public void UpdateCustomer(Customer customer)
    {
      using (SqlConnection conn = new SqlConnection(ConnectionString))
      {
        conn.Open();
        using (SqlCommand cmd = new SqlCommand(CustomerSQL.UpdateExistingCustomer, conn))
        {
          cmd.Parameters.AddWithValue("@customerId", customer.Id);
          cmd.Parameters.AddWithValue("@name", customer.Name);
          cmd.Parameters.AddWithValue("@address", customer.Address);
          cmd.Parameters.AddWithValue("@phone", customer.Phone);
          cmd.ExecuteNonQuery();
        }
      }
    }

    public List<Customer> GetAllCustomers()
    {
      List<Customer> customers = new List<Customer>();

      using (SqlConnection conn = new SqlConnection(ConnectionString))
      {
        conn.Open();
        using (SqlCommand cmd = new SqlCommand(CustomerSQL.GetAllCustomers, conn))
        {
          using (IDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              Customer customer = new Customer()
              {
                Id = Convert.ToInt64(reader["Id"]),
                Name = Convert.ToString(reader["Name"]),
                Address = Convert.ToString(reader["Address"]),
                Phone = Convert.ToString(reader["Phone"]),
              };
              customers.Add(customer);
            }
          }
        }
      }

      return customers;
    }

    public List<Customer> GetCustomersByAddress(string address)
    {
      List<Customer> customers = new List<Customer>();

      using (SqlConnection conn = new SqlConnection(ConnectionString))
      {
        conn.Open();
        using (SqlCommand cmd = new SqlCommand(CustomerSQL.GetCustomersByAddress, conn))
        {
          cmd.Parameters.AddWithValue("@address", address);

          using (IDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              Customer customer = new Customer()
              {
                Id = Convert.ToInt64(reader["Id"]),
                Name = Convert.ToString(reader["Name"]),
                Address = Convert.ToString(reader["Address"]),
                Phone = Convert.ToString(reader["Phone"]),
              };
              customers.Add(customer);
            }
          }
        }
      }

      return customers;
    }

    public void DeleteAllCustomers()
    {
      using (SqlConnection conn = new SqlConnection(ConnectionString))
      {
        conn.Open();
        using (SqlCommand cmd = new SqlCommand(CustomerSQL.DeleteAllCustomers, conn))
        {
          cmd.ExecuteNonQuery();
        }
      }
    }

    public void DeleteCustomersByAddress(string address)
    {
      using (SqlConnection conn = new SqlConnection(ConnectionString))
      {
        conn.Open();
        using (SqlCommand cmd = new SqlCommand(CustomerSQL.DeleteCustomersByAddress, conn))
        {
          cmd.Parameters.AddWithValue("@address", address);
          cmd.ExecuteNonQuery();
        }
      }
    }
  }
}
