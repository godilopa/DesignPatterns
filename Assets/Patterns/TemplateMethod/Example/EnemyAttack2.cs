
using UnityEngine;

namespace TemplateMethodExample
{
  public class EnemyAttack2 : TemplateMethod
  {
    [SerializeField]
    GameObject target = null;

    protected override void RequiredOperation1()
    {
      Debug.LogFormat("Enemy2 attacks to {0}", target.name);
    }

    protected override void RequiredOperation2()
    {
      Debug.LogFormat("Enemy2 hides from {0}", target.name);
    }

    [ContextMenu("Attack")]
    private void Attack()
    {
      ExecuteMethod();
    }
  }
}
