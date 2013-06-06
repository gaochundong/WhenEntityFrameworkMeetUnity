using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WhenEntityFrameworkMeetUnity.Models.Mapping;

namespace WhenEntityFrameworkMeetUnity.Models
{
    public partial class RETAILContext : DbContext
    {
        static RETAILContext()
        {
            Database.SetInitializer<RETAILContext>(null);
        }

        public RETAILContext()
            : base("Name=RETAILContext")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new OrderDetailMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new ShipperMap());
            modelBuilder.Configurations.Add(new SupplierMap());
        }
    }
}
