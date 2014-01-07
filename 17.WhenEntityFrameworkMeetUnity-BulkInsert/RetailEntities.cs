using System.Data.Entity;

namespace WhenEntityFrameworkMeetUnity
{
  public class RetailEntities : DbContext
  {
    static RetailEntities()
    {
      Database.SetInitializer<RetailEntities>(
        new CreateDatabaseIfNotExists<RetailEntities>());
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
