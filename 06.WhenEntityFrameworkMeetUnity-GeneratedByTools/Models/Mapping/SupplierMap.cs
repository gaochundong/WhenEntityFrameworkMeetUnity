using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WhenEntityFrameworkMeetUnity.Models.Mapping
{
    public class SupplierMap : EntityTypeConfiguration<Supplier>
    {
        public SupplierMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CompanyName)
                .IsRequired();

            this.Property(t => t.ContactName)
                .HasMaxLength(1024);

            this.Property(t => t.ContactTitle)
                .HasMaxLength(1024);

            // Table & Column Mappings
            this.ToTable("Supplier", "STORE");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.ContactName).HasColumnName("ContactName");
            this.Property(t => t.ContactTitle).HasColumnName("ContactTitle");
            this.Property(t => t.Address).HasColumnName("Address");
        }
    }
}
