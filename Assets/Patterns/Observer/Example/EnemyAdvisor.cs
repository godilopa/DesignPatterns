
using UnityEngine;

namespace CommandObserverExample
{
  public class EnemyAdvisor : Advisor<Enemy>
  {
    [ContextMenu("NotifyEnemy")]
    private void NotifyEnemy()
    {
      Notify(NotifyType.NOTIFY1);
      Notify(NotifyType.NOTIFY2);
    }
  }
}
