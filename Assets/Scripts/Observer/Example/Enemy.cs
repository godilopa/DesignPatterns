
using UnityEngine;

namespace CommandObserverExample
{
  public class Enemy : MonoBehaviour, IObserver
  {
    public void OnNotify(NotifyType type)
    {
      switch (type)
      {
        case NotifyType.NOTIFY1:
          Debug.Log("Enemy Notify 1");
          break;
        case NotifyType.NOTIFY2:
          Debug.Log("Enemy Notify 2");
          break;
        case NotifyType.NOTIFY3:
          Debug.Log("Enemy Notify 3");
          break;
        case NotifyType.NOTIFY4:
          Debug.Log("Enemy Notify 4");
          break;
      }
    }
  }
}
