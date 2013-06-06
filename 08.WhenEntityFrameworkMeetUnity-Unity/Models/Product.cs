using System;
using System.Collections.Generic;

namespace WhenEntityFrameworkMeetUnity.Models
{
    public partial class Product
    {
        public Product()
        {
            this.OrderDetails = new List<OrderDetail>();
        }

        public long Id { get; set; }
        public long CategoryId { get; set; }
        public long SupplierId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
