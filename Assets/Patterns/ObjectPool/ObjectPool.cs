using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
  [System.Serializable]
  private class Pool
  {
    public List<GameObject> Despawned
    {
      get { return despawned; }
      set { despawned = value; }
    }

    public List<GameObject> Spawned
    {
      get { return spawned; }
      set { spawned = value; }
    }

    public GameObject prefab = null;

    public string id = string.Empty;

    public int count = 0;

    private List<GameObject> spawned;

    private List<GameObject> despawned;

    private GameObject go;

    public GameObject PullFromPool(GameObject container, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), bool instantiateIfFull = false)
    {
      if (despawned.Count > 0)
      {
        go = despawned[0];

        despawned.Remove(go);
        spawned.Add(go);

        go.SetActive(true);
        go.transform.position = position;
        go.transform.rotation = rotation;
      }
      else if (instantiateIfFull == true)
      {
        GameObject newObj = Instantiate(prefab, position, rotation, container.transform) as GameObject;
        spawned.Add(newObj);
      }

      if (go == null)
        Debug.LogWarningFormat("Invalid object in pool '{0}'.", go.name);
      else
        Debug.LogFormat("Spawn object '{0}' in pool. Position {1}: Rotation: {2}", go.name, position, rotation);

      return go;
    }

    public void PushToPool(GameObject go)
    {
      go.SetActive(false);
      despawned.Add(go);
      spawned.Remove(go);
      Debug.LogFormat("Despawn object '{0}'to pool.", go.name);
    }
  }

  [SerializeField]
  private List<Pool> pools = null;

  [SerializeField]
  private int defaultBufferAmount = 3;

  private GameObject containerObject;

  public GameObject PullFromPool(string id, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), bool instantiateIfFull = false)
  {
    for (int i = 0; i < pools.Count; i++)
    {
      if (pools[i].id == id)
        return pools[i].PullFromPool(containerObject, position, rotation, instantiateIfFull);
    }

    Debug.LogWarningFormat("Invalid id '{0}'.", id);

    return null;
  }

  public void PushToPool(GameObject go, bool destroIfFail = false)
  {
    bool destroy = true;

    for (int i = 0; i < pools.Count; i++)
    {
      if (pools[i].Spawned.Contains(go))
      {
        pools[i].PushToPool(go);
        destroy = false;
      }
    }

    if (destroIfFail == true && destroy == true)
    {
      Destroy(go);
      Debug.LogWarningFormat("Destroyed '{0}' from pool.", go.name);
    }
  }

  private void FillPools()
  {
    for (int i = 0; i < pools.Count; i++)
    {
      int size = pools[i].count;

      if (size != 0)
      {
        pools[i].Despawned = new List<GameObject>();
        pools[i].Spawned = new List<GameObject>();
        containerObject = new GameObject(pools[i].id);
        containerObject.transform.parent = transform;

        if (size < defaultBufferAmount)
          size = 3;
      }

      for (int j = 0; j < size; j++)
      {
        GameObject newObj = Instantiate(pools[i].prefab, containerObject.transform) as GameObject;
        newObj.name = string.Format("{0} {1}", pools[i].id, j);
        newObj.SetActive(false);
        pools[i].Despawned.Add(newObj);
      }
    }
  }

  private void Start()
  {
    FillPools();
  }
}