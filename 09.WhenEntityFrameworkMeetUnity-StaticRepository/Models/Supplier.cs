using System;
using System.Collections.Generic;

namespace WhenEntityFrameworkMeetUnity.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            this.Products = new List<Product>();
        }

        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
