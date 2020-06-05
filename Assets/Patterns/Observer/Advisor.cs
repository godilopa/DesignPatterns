
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Advisor<S> : MonoBehaviour where S : IObserver
{
  private static List<S> observers = new List<S>();

  private List<T> Find<T>()
  {
    List<T> interfaces = new List<T>();
    GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
    for (int i = 0; i < rootGameObjects.Length; i++)
    {
      T[] childrenInterfaces = rootGameObjects[i].GetComponentsInChildren<T>();

      for (int j = 0; j < childrenInterfaces.Length; j++)
        interfaces.Add(childrenInterfaces[j]);
    }

    return interfaces;
  }

  protected virtual void Notify(NotifyType type)
  {
    for (int i = 0; i < observers.Count; i++)
    {
      Debug.LogFormat("Advisor notify {0} to observers", type.ToString());
      observers[i].OnNotify(type);
    }
  }

  protected virtual void Awake()
  {
    //If observers havent been filled yet
    if (observers.Count == 0)
    {
      observers = Find<S>();
      Debug.LogFormat("Advisor filling observers of type {0}", typeof(S));
    }
  }

  protected virtual void OnDestroy()
  {
    observers.Clear();
  }
}
