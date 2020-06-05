
using UnityEngine;

namespace CommandObserverExample
{
  public class BoostAdvisor : Advisor<IObserver>
  {
    [ContextMenu("Notify1PlayerAndEnemy")]
    private void NotifyPlayerAndEnemy()
    {
      Notify(NotifyType.NOTIFY1);
    }
  }
}
