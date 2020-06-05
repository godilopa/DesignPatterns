
using UnityEngine;

namespace ChainOfResponsabilityExample
{
  public class Boss : Handler
  {
    public override void ReplyMessage()
    {
      Debug.Log("Boss answering");
    }
  }
}
