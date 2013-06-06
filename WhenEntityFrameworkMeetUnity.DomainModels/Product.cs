using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhenEntityFrameworkMeetUnity.DomainModels
{
  public class Product
  {
    public long Id { get; set; }
    public string Name { get; set; }

    public Category Category { get; set; }
    public Supplier Supplier { get; set; }

    public float UnitPrice { get; set; }
    public int UnitsInStock { get; set; }

    public override string ToString()
    {
      return string.Format("Id[{0}], Name[{1}], Category[{2}], Supplier[{3}], UnitPrice[{4}], UnitsInStock[{5}]",
        Id, Name, Category, Supplier, UnitPrice, UnitsInStock);
    }
  }
}
