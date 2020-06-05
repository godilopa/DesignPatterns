using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MediatorExample
{
  public class TrafficLight : MonoBehaviour, IMediator
  {
    [SerializeField]
    private Person component1 = null;

    [SerializeField]
    private Car component2 = null;

    public void Mediate(string operation)
    {
      switch (operation)
      {
        case "Car":
          Debug.Log("You can move now");
          break;
        case "Person":
          Debug.Log("You cant move now");
          break;
      }
    }

    private void Awake()
    {
      component1.SetMediator(this);
      component2.SetMediator(this);
    }
  }
}
