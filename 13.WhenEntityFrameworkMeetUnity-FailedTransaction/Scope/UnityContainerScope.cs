using System;
using System.Collections.Concurrent;
using System.Threading;

namespace WhenEntityFrameworkMeetUnity
{
  public class UnityContainerScope : IDisposable
  {
    private static ConcurrentDictionary<int, bool> scopeMapping
      = new ConcurrentDictionary<int, bool>();
    private static bool allowedDispose = true;

    protected UnityContainerScope()
    {
      ScopeId = Thread.CurrentThread.ManagedThreadId;
      scopeMapping.Add(ScopeId, true);
    }

    public int ScopeId { get; private set; }
    public static int ScopeCount { get { return scopeMapping.Count; } }

    public static UnityContainerScope NewScope()
    {
      return new UnityContainerScope();
    }

    public static void RunInScope(Action action)
    {
      if (action == null)
        throw new ArgumentNullException("action");

      using (UnityContainerScope scope = UnityContainerScope.NewScope())
      {
        action.Invoke();
      }
    }

    public static T RunInScope<T>(Func<T> function)
    {
      if (function == null)
        throw new ArgumentNullException("function");

      using (UnityContainerScope scope = UnityContainerScope.NewScope())
      {
        return function.Invoke();
      }
    }

    public static bool InScope(int scopeId)
    {
      return scopeMapping.ContainsKey(scopeId);
    }

    public static void InterceptDisposing(bool isAllowedDispose)
    {
      allowedDispose = isAllowedDispose;
    }

    public void Dispose()
    {
      if (allowedDispose)
      {
        UnityContainerDispatcher.DisposeContainer();
        scopeMapping.Remove(ScopeId);
      }
    }
  }
}
