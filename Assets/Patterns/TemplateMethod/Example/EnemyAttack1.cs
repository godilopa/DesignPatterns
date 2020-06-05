
using UnityEngine;

namespace TemplateMethodExample
{
  public class EnemyAttack1 : TemplateMethod
  {
    [SerializeField]
    GameObject target;

    protected override void RequiredOperation1()
    {
      Debug.LogFormat("Enemy1 attacks to {0}", target.name);
    }

    protected override void RequiredOperation2()
    {
      Debug.LogFormat("Enemy1 attacks Again");
    }

    protected override void OptionalOperation1()
    {
      Debug.LogFormat("Enemy1 getting closer to {0}", target.name);
    }

    [ContextMenu("Attack")]
    private void Attack()
    {
      ExecuteMethod();
    }
  }
}
