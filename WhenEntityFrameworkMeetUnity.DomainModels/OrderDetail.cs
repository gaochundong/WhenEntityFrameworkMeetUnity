using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhenEntityFrameworkMeetUnity.DomainModels
{
  public class OrderDetail
  {
    public long Id { get; set; }

    public Order Order { get; set; }
    public Product Product { get; set; }

    public int Quantity { get; set; }
    public float Discount { get; set; }

    public override string ToString()
    {
      return string.Format("Id[{0}], Quantity[{1}], Discount[{2}]",
        Id, Discount, Discount);
    }
  }
}
