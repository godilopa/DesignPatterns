using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandObserverExample
{
  public class AlarmAdvisor : Advisor<IObserver>
  {
    [ContextMenu("Notify3PlayerAndEnemy")]
    private void NotifyPlayerAndEnemy()
    {
      Notify(NotifyType.NOTIFY3);
    }
  }
}

