using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MediatorExample
{
  public class Car : Mediable
  {
    [ContextMenu("AskForMove")]
    private void AskForMove()
    {
      mediator.Mediate("Car");
    }
  }
}
