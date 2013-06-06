using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace WhenEntityFrameworkMeetUnity
{
  public class RetailEntities : DbContext
  {
    static RetailEntities()
    {
      //Database.SetInitializer<RetailEntities>(new CreateDatabaseIfNotExists<RetailEntities>());
      //Database.SetInitializer<RetailEntities>(new DropCreateDatabaseAlways<RetailEntities>());
      //Database.SetInitializer<RetailEntities>(new DropCreateDatabaseIfModelChanges<RetailEntities>());
      Database.SetInitializer<RetailEntities>(null);
    }

    public RetailEntities()
      : base("Name=RetailEntities")
    {
    }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Configurations.Add(new CustomerMap());
    }
  }
}
