
using UnityEngine;

namespace ChainOfResponsabilityExample
{
  public class Enemy2 : Handler
  {
    public override void ReplyMessage()
    {
      Debug.Log("Enemy2 answering");
    }
  }
}
