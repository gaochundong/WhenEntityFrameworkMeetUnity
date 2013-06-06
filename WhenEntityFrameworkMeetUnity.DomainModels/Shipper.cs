using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhenEntityFrameworkMeetUnity.DomainModels
{
  public class Shipper
  {
    public long Id { get; set; }
    public string CompanyName { get; set; }
    public string ContactName { get; set; }
    public string Phone { get; set; }

    public override string ToString()
    {
      return string.Format("Id[{0}], CompanyName[{1}], ContactName[{2}], Phone[{3}]",
        Id, CompanyName, ContactName, Phone);
    }
  }
}
