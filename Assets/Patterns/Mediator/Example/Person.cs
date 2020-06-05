using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MediatorExample
{
  public class Person : Mediable
  {
    [ContextMenu("AskForMove")]
    private void AskForMove()
    {
      mediator.Mediate("Person");
    }
  }
}
