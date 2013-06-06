using System;
using System.Collections.Concurrent;
using System.Threading;

namespace WhenEntityFrameworkMeetUnity
{
  public class UnityContainerScope : IDisposable
  {
    private static ConcurrentDictionary<int, bool> scopeMapping
      = new ConcurrentDictionary<int, bool>();

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

    public static bool InScope(int scopeId)
    {
      return scopeMapping.ContainsKey(scopeId);
    }

    public void Dispose()
    {
      UnityContainerDispatcher.DisposeContainer();
      scopeMapping.Remove(ScopeId);
    }
  }
}
