using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

namespace WhenEntityFrameworkMeetUnity
{
  public class CustomerMap : EntityTypeConfiguration<Customer>
  {
    public CustomerMap()
    {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Name)
          .IsRequired()
          .HasMaxLength(256);

      this.Property(t => t.Phone)
          .IsRequired()
          .HasMaxLength(256);

      // Table & Column Mappings
      this.ToTable("Customer", "STORE");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Name).HasColumnName("Name");
      this.Property(t => t.Address).HasColumnName("Address");
      this.Property(t => t.Phone).HasColumnName("Phone");

      // Relationships
      //this.HasRequired(t => t.Status)
      //    .WithMany(t => t.CustomerStatus)
      //    .HasForeignKey(d => d.Status);
    }
  }
}
