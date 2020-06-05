
using UnityEngine;

namespace CommandObserverExample
{
  public class Player : MonoBehaviour, IObserver
  {
    public void OnNotify(NotifyType type)
    {
      switch (type)
      {
        case NotifyType.NOTIFY1:
          Debug.Log("Player Notify 1");
          break;
        case NotifyType.NOTIFY2:
          Debug.Log("Player Notify 2");
          break;
        case NotifyType.NOTIFY3:
          Debug.Log("Player Notify 3");
          break;
        case NotifyType.NOTIFY4:
          Debug.Log("Player Notify 4");
          break;
      }
    }
  }
}
