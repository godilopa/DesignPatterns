using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FactoryMethodExample
{
  public class Client : MonoBehaviour
  {
    [SerializeField]
    private Factory factory = null;

    [ContextMenu("Create Cube")]
    private void Spawn1()
    {
      factory.GetInstance("Cube");
    }

    [ContextMenu("Create Sphere")]
    private void Spawn2()
    {
      factory.GetInstance("Sphere");
    }
  }
}
