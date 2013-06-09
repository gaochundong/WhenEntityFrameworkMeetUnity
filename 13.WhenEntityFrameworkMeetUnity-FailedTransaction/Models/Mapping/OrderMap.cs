using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WhenEntityFrameworkMeetUnity.Models.Mapping
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ShipAddress)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Order", "STORE");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.ShipperId).HasColumnName("ShipperId");
            this.Property(t => t.OrderDate).HasColumnName("OrderDate");
            this.Property(t => t.TotalCharge).HasColumnName("TotalCharge");
            this.Property(t => t.Freight).HasColumnName("Freight");
            this.Property(t => t.ShipDate).HasColumnName("ShipDate");
            this.Property(t => t.ShipAddress).HasColumnName("ShipAddress");

            // Relationships
            this.HasRequired(t => t.Customer)
                .WithMany(t => t.Orders)
                .HasForeignKey(d => d.CustomerId);
            this.HasRequired(t => t.Shipper)
                .WithMany(t => t.Orders)
                .HasForeignKey(d => d.ShipperId);

        }
    }
}
