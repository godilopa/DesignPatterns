using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mediable : MonoBehaviour
{
  protected IMediator mediator;

  public void SetMediator(IMediator mediator)
  {
    this.mediator = mediator;
  }
}
