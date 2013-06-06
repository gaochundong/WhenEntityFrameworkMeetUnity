using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace WhenEntityFrameworkMeetUnity
{
  public static class ObjectContextExtensions
  {
    public static EntitySetBase GetEntitySet<T>(this ObjectContext context)
    {
      EntityContainer container =
              context.MetadataWorkspace
                  .GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace);
      Type baseType = GetBaseType(typeof(T));
      EntitySetBase entitySet = container.BaseEntitySets
          .Where(item => item.ElementType.Name.Equals(baseType.Name))
          .FirstOrDefault();

      return entitySet;
    }

    private static Type GetBaseType(Type type)
    {
      var baseType = type.BaseType;
      if (baseType != null && baseType != typeof(EntityObject))
      {
        return GetBaseType(type.BaseType);
      }
      return type;
    }
  }
}
