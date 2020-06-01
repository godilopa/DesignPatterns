
using UnityEngine;

public class Factory : MonoBehaviour
{
  [System.Serializable]
  private struct FactoryObjects
  {
    public GameObject prefab;
    public string id;
  }

  [SerializeField]
  private FactoryObjects[] factoryObjects = null;

  public GameObject GetInstance(string id, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), Transform father = null)
  {
    GameObject go = null;
    string name = string.Empty;

    for (int i = 0; i < factoryObjects.Length; i++)
    {
      //If you dont choose a name, prefab name is used instead
      name = factoryObjects[i].id == string.Empty ? factoryObjects[i].prefab.name : factoryObjects[i].id;

      if (name == id)
      {
        go = factoryObjects[i].prefab;
        break;
      }
    }

    if (go == null)
    {
      Debug.LogFormat("FactoryMethod '{0}' not found", id);
      return null;
    }

    Debug.LogFormat("FactoryMethod instantiating '{0}' Position:{1} Rotation:{2} Father:{3}", id, position, rotation, father);
    return Instantiate(go, position, rotation, father);
  }
}
