using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhenEntityFrameworkMeetUnity.DomainModels
{
  public class Category
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public override string ToString()
    {
      return string.Format("Id[{0}], Name[{1}], Description[{2}]",
        Id, Name, Description);
    }
  }
}
