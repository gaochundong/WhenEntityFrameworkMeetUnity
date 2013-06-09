using System;

namespace WhenEntityFrameworkMeetUnity
{
  [Serializable]
  public class UnityContainerNotInScopeException : Exception
  {
    public UnityContainerNotInScopeException()
      : base()
    {
    }

    public UnityContainerNotInScopeException(string message)
      : base(message)
    {
    }

    public UnityContainerNotInScopeException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
