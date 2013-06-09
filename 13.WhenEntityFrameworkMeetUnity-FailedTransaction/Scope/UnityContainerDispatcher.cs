using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Threading;
using Microsoft.Practices.Unity;

namespace WhenEntityFrameworkMeetUnity
{
  public static class UnityContainerDispatcher
  {
    private static IUnityContainer parentContainer = null;
    private static ConcurrentDictionary<int, IUnityContainer> containerMapping
      = new ConcurrentDictionary<int, IUnityContainer>();
    private static bool allowedDispose = true;

    public static void InjectParentContainer(IUnityContainer unity)
    {
      if (unity == null)
        throw new ArgumentNullException("unity");

      parentContainer = unity;
    }

    public static void InjectContainer(int key, IUnityContainer unity)
    {
      if (unity == null)
        throw new ArgumentNullException("unity");

      containerMapping.Add(key, unity);
    }

    public static void InterceptDisposing(bool isAllowedDispose)
    {
      allowedDispose = isAllowedDispose;
    }

    public static IUnityContainer GetParentContainer()
    {
      return parentContainer;
    }

    public static IUnityContainer GetContainer()
    {
      int key = Thread.CurrentThread.ManagedThreadId;

      if (!UnityContainerScope.InScope(key))
      {
        throw new UnityContainerNotInScopeException(
          string.Format(CultureInfo.InvariantCulture,
          "The specified scope id [{0}] is not in scope.", key));
      }

      if (!containerMapping.ContainsKey(key))
      {
        BuildUpContainer(key);
      }

      return containerMapping.Get(key);
    }

    public static void DisposeContainer()
    {
      int key = Thread.CurrentThread.ManagedThreadId;
      IUnityContainer container = containerMapping.Remove(key);
      if (container != null && allowedDispose)
      {
        container.Dispose();
      }
    }

    private static void BuildUpContainer(int key)
    {
      if (parentContainer == null)
        throw new InvalidProgramException("The parent container cannot be null.");

      IUnityContainer childContainer = parentContainer.CreateChildContainer();
      containerMapping.Add(key, childContainer);
    }
  }
}
