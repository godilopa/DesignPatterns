using UnityEngine;

namespace CommandObjectPoolExample
{
  public class Client : MonoBehaviour
  {
    [SerializeField]
    private ObjectPool objectPool = null;

    [SerializeField]
    private string idObject = string.Empty;

    private GameObject objectSpawned;

    [ContextMenu("Spawn")]
    private void Spawn()
    {
      objectSpawned = objectPool.PullFromPool(idObject);
    }

    [ContextMenu("Despawn")]
    private void Despawn()
    {
      objectPool.PushToPool(objectSpawned);
    }
  }
}
