using System;
using System.Collections.Generic;

namespace WhenEntityFrameworkMeetUnity.Models
{
    public partial class Order
    {
        public Order()
        {
            this.OrderDetails = new List<OrderDetail>();
        }

        public long Id { get; set; }
        public long CustomerId { get; set; }
        public long ShipperId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public double TotalCharge { get; set; }
        public double Freight { get; set; }
        public System.DateTime ShipDate { get; set; }
        public string ShipAddress { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Shipper Shipper { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
