using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhenEntityFrameworkMeetUnity.DomainModels
{
  public class Supplier
  {
    public long Id { get; set; }
    public string CompanyName { get; set; }
    public string ContactName { get; set; }
    public string ContactTitle { get; set; }
    public string Address { get; set; }

    public override string ToString()
    {
      return string.Format("Id[{0}], CompanyName[{1}], ContactName[{2}], ContactTitle[{3}], Address[{4}]",
        Id, CompanyName, ContactName, ContactTitle, Address);
    }
  }
}
