using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhenEntityFrameworkMeetUnity.DomainModels
{
  public class Customer
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }

    public override string ToString()
    {
      return string.Format("Id[{0}], Name[{1}], Address[{2}], Phone[{3}]",
        Id, Name, Address, Phone);
    }
  }
}
