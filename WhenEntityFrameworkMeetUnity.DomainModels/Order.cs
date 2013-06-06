using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhenEntityFrameworkMeetUnity.DomainModels
{
  public class Order
  {
    public long Id { get; set; }
    public DateTime OrderDate { get; set; }

    public Customer Customer { get; set; }
    public float TotalCharge { get; set; }
    public float Freight { get; set; }

    public OrderDetail Detail { get; set; }

    public Shipper Shipper { get; set; }
    public DateTime ShipDate { get; set; }
    public string ShipAddress { get; set; }

    public override string ToString()
    {
      return string.Format("Id[{0}], OrderDate[{1}], Customer[{2}], TotalCharge[{3}], Freight[{4}], Detail[{5}], Shipper[{6}], ShipDate[{7}], ShipAddress[{8}]",
        Id, OrderDate, Customer, TotalCharge, Freight, Detail, Shipper, ShipDate, ShipAddress);
    }
  }
}
